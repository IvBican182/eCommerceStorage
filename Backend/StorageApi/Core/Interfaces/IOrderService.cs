

using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;


namespace StorageApi.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Cart cart, Guid userId);
        Task<Order> RemoveOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products);
        Task<Order> AddOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products);
        Task<bool> DeleteOrder(Guid orderId, Guid userId);
    }
}