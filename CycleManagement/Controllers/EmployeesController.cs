using System.Data;
using CycleManagement.Data;
using CycleManagement.DTO.EmployeesDTO;
using CycleManagement.Models;
using CycleManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CycleManagement.Controllers
{

    public class EmployeesController : BaseController<Employee>
    {
        AuthenticationServices _authService;
        ApplicationDbContext _dbContext;
        public EmployeesController(ApplicationDbContext context, AuthenticationServices authService) : base(context)
        {
            _dbContext = context;
            _authService = authService;
        }

        [NonAction]
        public override Task<ActionResult<Employee>> Create([FromBody] Employee entity)
        {
            return base.Create(entity);
        }

        [NonAction]
        public override Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return base.GetAll();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll([FromQuery] string search)
        {
            IQueryable<Employee> query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(employee => employee.Name.Contains(search));
            }

            IEnumerable<Employee> result = await query.ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Create([FromBody] NewEmployeeRequest entity)
        {
            Employee employeeEntity = new Employee
            {
                Email = entity.Email,
                Phone = entity.Phone,
                Name = entity.Name
            };


            try
            {
                _dbSet.Add(employeeEntity);
            }
            catch (SqlException e)
            {
                if (e.ErrorCode == 234)
                {
                    return BadRequest("Email or Phone Nubmber already in use");
                }
            }

            try
            {
                LoginCredential loginEntity = new LoginCredential
                {
                    Id = employeeEntity.Id,
                    LoginId = entity.LoginId,
                    PasswordHash = _authService.HashPassword(entity.PlaintextPassword),
                    Role = entity.Role
                };

                await _dbContext.Set<LoginCredential>().AddAsync(loginEntity);
            }
            catch
            {
                return BadRequest("Login Id already in use");
            }

            await _dbContext.SaveChangesAsync();

            var entityId = _context.Entry(employeeEntity).Property("Id").CurrentValue; // Get generated ID

            return CreatedAtAction(nameof(GetById), new { id = entityId }, entity);
        }
    }
}
