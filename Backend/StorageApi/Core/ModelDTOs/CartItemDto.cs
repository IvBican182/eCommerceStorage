

namespace StorageApi.Core.ModelsDTO
{
    public class CartItemDto
    {
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal CartItemPrice { get; set; }
    }
}