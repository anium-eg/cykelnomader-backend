using System.Text.Json.Serialization;
using CycleManagement.Models;

namespace CycleManagement.DTO.OrderDTO
{
    public class NewOrderRequest
    {
        public Order Order { get; set; }

        public Customer Customer { get; set; }
       
    }
}
