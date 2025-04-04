using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
