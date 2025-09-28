

using System.ComponentModel.DataAnnotations.Schema;


namespace StorageApi.Core.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public Guid CartStatusId { get; set; }

        [ForeignKey(nameof(CartStatusId))]
        public CartStatus CartStatus { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

        public Order Order { get; set; }

        [NotMapped]
        public decimal CartTotal
        {
            get
            {
                decimal total = 0;
                foreach (var i in CartItems)
                {
                    total += i.CartItemPrice;
                }
                return total;
            }
        }
    }
}