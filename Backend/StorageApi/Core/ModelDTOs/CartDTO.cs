



namespace StorageApi.Core.ModelsDTO
{
    public class CartDTO
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

    }
}