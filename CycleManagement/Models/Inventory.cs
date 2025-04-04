using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class Inventory
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
        public bool IsOnDisplay { get; set; }

        // Navigation property
        public Product Product { get; set; }
    }

}
