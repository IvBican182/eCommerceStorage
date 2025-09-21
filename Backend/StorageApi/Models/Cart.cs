

using System.ComponentModel.DataAnnotations.Schema;

namespace StorageApi.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

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