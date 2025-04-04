using CycleManagement.Data;
using CycleManagement.DTO.OrderDTO;
using CycleManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CycleManagement.Controllers
{
    public class OrdersController : BaseController<Order>
    {
        public OrdersController(ApplicationDbContext context) : base(context)
        {
        }

        [NonAction]
        public override Task<ActionResult<Order>> Create([FromBody] Order entity)
        {
            return base.Create(entity);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] NewOrderRequest request)
        {
            Customer customerEntity = request.Customer;
            Order orderEntity = request.Order;


            try
            {
                await _context.Set<Customer>().AddAsync(customerEntity);
                await _context.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                return BadRequest("Customer already existe");
            }

            try
            {
                await _context.Set<Order>().AddAsync(orderEntity);
                await _context.SaveChangesAsync();
            }
            catch(SqlException e)
            {
                return BadRequest("Fault with order table");
            }

            return Ok();


            //else
            //{
            //    orderEntity.CustomerId = customerEntity.Id;
            //}


        }

        
    }
}
