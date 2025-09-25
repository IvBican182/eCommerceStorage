


namespace StorageApi.Core.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;
        public int ItemQuantity { get; set; }
        public decimal Price { get; set; }
    }
}