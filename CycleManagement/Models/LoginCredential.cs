using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class LoginCredential
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public string LoginId { get; set; }
        public string PasswordHash { get; set; } // Store the hash of the password
        public string Role { get; set; }
    }
}
