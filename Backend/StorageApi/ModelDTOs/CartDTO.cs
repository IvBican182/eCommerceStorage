

namespace StorageApi.ModelsDTO
{
    public class CartDTO
    { 
        public decimal TotalPrice { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

    }
}