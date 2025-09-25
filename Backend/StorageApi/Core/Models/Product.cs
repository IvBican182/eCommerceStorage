

using System.ComponentModel.DataAnnotations;

namespace StorageApi.Core.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }


    }
}