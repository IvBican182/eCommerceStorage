

using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;


namespace StorageApi.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Guid cartId, Guid userId);
        Task<Order> RemoveOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products);
        Task<bool> DeleteOrder(Guid orderId, Guid userId);
        Task CheckoutOrder(Guid userId, Guid orderId);
        Task<Order> AddOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products);
    }
}