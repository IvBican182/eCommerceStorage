
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;

namespace StorageApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.GetRepository<Order>();
            _cartRepository =  _unitOfWork.GetRepository<Cart>();
        }

        //POTREBNO DORADITI radi dodatka statusa narudžbe kako bi znali o kojoj točno narudžbi korisnika se radi
        //prilikom akcija na narudžbi
        public async Task<Order> CreateOrder(Cart cart, Guid userId)
        {
            var userCart = await _cartRepository.GetByIdAsync(cart.Id);

            if (userCart.CartStatus.Name != "Active")
            {
                throw new KeyNotFoundException("order not found or was already completed/cancelled");
            }

            var order = new Order
            {
                UserId = cart.UserId,
                TotalPrice = cart.CartTotal
            };

            foreach (var cartItem in cart.CartItems)
            {

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.Id,
                    ItemQuantity = cartItem.Quantity,
                    Price = cartItem.CartItemPrice
                };

                order.OrderItems.Add(orderItem);
            }

            _orderRepository.Add(order);

            await _unitOfWork.SaveChangesAsync();

            return order;

        }
        public async Task<Order> AddOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            var userOrder = await _orderRepository.GetAll().Where(o => o.Id == orderId && o.UserId == userId).FirstOrDefaultAsync();

            if (userOrder == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            foreach (var product in products)
            {
                var orderItem = userOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == product.ProductId);

                orderItem.ItemQuantity += product.quantity;

            }

            await _unitOfWork.SaveChangesAsync();

            return userOrder;
            
        }

        public async Task<Order> RemoveOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            var userOrder = await _orderRepository.GetAll().Where(o => o.Id == orderId && o.UserId == userId).FirstOrDefaultAsync();

            if (userOrder == null)
            {
                throw new KeyNotFoundException("order not found");
            }

            foreach (var product in products)
            {
                var orderItem = userOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == product.ProductId);

                orderItem.ItemQuantity -= product.quantity;

                if (orderItem.ItemQuantity <= 0)
                {
                    userOrder.OrderItems.Remove(orderItem);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return userOrder;


        }

        public async Task<bool> DeleteOrder(Guid orderId, Guid userId)
        {
            var userOrder = await _orderRepository.GetAll().Where(o => o.Id == orderId && o.UserId == userId).Include(oi => oi.OrderItems).FirstOrDefaultAsync();

            if (userOrder == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            _orderRepository.Remove(userOrder);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}