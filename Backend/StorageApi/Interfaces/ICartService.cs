


using StorageApi.Models;

namespace StorageApi.Interfaces
{
    public interface ICartService
    {
        Task<Cart> CreateCart(Guid userId, List<(Guid productId, int quantity)> products);
        Task<Cart> GetUserCart(Guid userId);
        Task<Cart> AddItemToCart(Guid cartId, List<(Guid productId, int quantity)> products);
        Task<Cart> RemoveItemFromCart(Guid cartId, List<(Guid productId, int quantity)> products);
        Task<bool> DeleteCart(Guid cartId, Guid userId);
    }
}