using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CycleManagement.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        //public int Price { get; set; }
        public string? Customization { get; set; }
        public string Status { get; set; } = "Payment Pending";

        // Navigation properties
        [JsonIgnore]
        public Product? Product { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public ICollection<Payment>? Payments { get; set; }
    }
}
