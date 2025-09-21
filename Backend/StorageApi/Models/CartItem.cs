

using System.ComponentModel.DataAnnotations.Schema;

namespace StorageApi.Models
{
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        [NotMapped]
        public decimal CartItemPrice
        {
            get
            {
                if (Product == null)
                {
                    return 0;
                }

                return Product.Price * Quantity;
            }
        }

    }
}