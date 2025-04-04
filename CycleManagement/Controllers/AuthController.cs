using System.Diagnostics;
using CycleManagement.Data;
using CycleManagement.DTO.AuthDTO;
using CycleManagement.Models;
using CycleManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CycleManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationServices _authService;
        private readonly ApplicationDbContext _dbContext;

        public AuthController(AuthenticationServices authService, ApplicationDbContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<SuccessfulLoginResponse>> Login([FromBody] LoginRequest LoginRequestBody)
        {
            LoginCredential? loginCredentialEntity = await _dbContext.LoginCredentials.FirstOrDefaultAsync(user => user.LoginId == LoginRequestBody.LoginId);

            try
            {
                if (loginCredentialEntity != null &&
                _authService.HashPassword(LoginRequestBody.PlainTextPassword) == loginCredentialEntity.PasswordHash && 
                LoginRequestBody.RoleClaim == loginCredentialEntity.Role
                )
                {
                    Employee? employeeEntity = await _dbContext.Employees.FindAsync(loginCredentialEntity.Id);

                    if (employeeEntity != null)
                    {
                        SuccessfulLoginResponse result = new SuccessfulLoginResponse
                        {
                            Token = _authService.GenerateJwtToken(loginCredentialEntity.Id.ToString(), loginCredentialEntity.Role),
                            Employee = employeeEntity
                        };

                        return Ok(result);
                    }

                    else
                    {
                        return Problem("An unexpected error occurred.");
                    }
                }
                else
                {
                    return Unauthorized("Invalid Employee Id or Password");
                }
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
            
        }
    }
}
