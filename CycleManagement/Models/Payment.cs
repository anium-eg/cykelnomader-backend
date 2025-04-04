using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public Guid OrderId { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public string? Status { get; set; } = "Pending";

        // Navigation property
        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
