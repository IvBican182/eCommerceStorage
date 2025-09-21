
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageApi.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = new(); 
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal TotalPrice { get; set; }
    }
}