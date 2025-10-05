


using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;


namespace StorageApi.Core.Interfaces
{
    public interface ICartService
    {
        Task<Cart> CreateCart(Guid userId, List<AddRemoveCartItemDto> products);
        Task<Cart> GetUserCart(Guid userId);
        Task<Cart> AddItemToCart(Guid cartId, AddRemoveCartItemDto product);
        Task<Cart> RemoveItemFromCart(Guid cartId, List<AddRemoveCartItemDto> products);
        Task<DeleteCartResponseDto> DeleteCart(Guid cartId, Guid userId);
    }
}