using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
