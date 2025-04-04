using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public string Name { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
