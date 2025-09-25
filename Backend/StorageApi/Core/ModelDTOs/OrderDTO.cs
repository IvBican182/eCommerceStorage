

using StorageApi.Core.Models;

namespace StorageApi.Core.ModelsDTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
        
}
