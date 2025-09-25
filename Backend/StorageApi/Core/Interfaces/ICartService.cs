


using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;


namespace StorageApi.Core.Interfaces
{
    public interface ICartService
    {
        Task<Cart> CreateCart(Guid userId, List<AddRemoveCartItemDto> products);
        Task<Cart> GetUserCart(Guid userId);
        Task<Cart> AddItemToCart(Guid cartId, List<AddRemoveCartItemDto> products);
        Task<Cart> RemoveItemFromCart(Guid cartId, List<AddRemoveCartItemDto> products);
        Task<bool> DeleteCart(Guid cartId, Guid userId);
    }
}