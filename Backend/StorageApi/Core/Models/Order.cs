


using System.ComponentModel.DataAnnotations.Schema;

namespace StorageApi.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? CartId { get; set; } 
              
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; } 
        public Guid? OrderStatusId { get; set; }

        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; } = new(); 
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal TotalPrice { get; set; }
    }
}