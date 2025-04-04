using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CycleManagement.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; } // e.g., "ABC-0000"
        public Guid CategoryId { get; set; }
        public int Price { get; set; }
        public string PhotoURL { get; set; }

        // Navigation property
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
