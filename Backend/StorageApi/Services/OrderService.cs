
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;
using StorageApi.Core.Constants;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Http.HttpResults;

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
            _cartRepository = _unitOfWork.GetRepository<Cart>();
        }

        //POTREBNO DODATI - ConfirmOrder gdje će onda CartStatus postati completed, order također confirmed
        public async Task<Order> CreateOrder(Guid cartId, Guid userId)
        {
            var userCart = await _cartRepository
                                        .GetAll()
                                        .Where(c => c.UserId == userId && c.Id == cartId)
                                        .Include(c => c.Order)
                                        .Include(q => q.CartItems)
                                        .FirstOrDefaultAsync();
            if (userCart == null)
            {
                throw new KeyNotFoundException("user cart not found");
            }

            if (userCart.CartStatusId != CartStatusConstants.Active)
                {
                    throw new InvalidOperationException("Cart is not active.");
                }

            if (userCart.Order != null)
            {
                throw new KeyNotFoundException("cart already has a connected order");
            }

            var order = new Order
            {
                UserId = userCart.UserId,
                TotalPrice = userCart.CartTotal,
                CartId = userCart.Id,
                OrderStatusId = OrderStatusConstants.Active
            };

            foreach (var cartItem in userCart.CartItems)
            {

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
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
            var userOrder = await _orderRepository
                                            .Find(o => o.Id == orderId && o.UserId == userId)
                                            .Include(o => o.OrderItems)
                                            .Include(os => os.OrderStatus)
                                            .FirstOrDefaultAsync();

            if (userOrder == null || userOrder.OrderStatusId != OrderStatusConstants.Active)
            {
                throw new KeyNotFoundException("order was not found");
            }

            foreach (var product in products)
            {
                var orderItem = userOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == product.ProductId);

                if (orderItem != null)
                {
                    orderItem.ItemQuantity += product.quantity;
                }

            }

            await _unitOfWork.SaveChangesAsync();

            return userOrder;

        }

        public async Task<Order> RemoveOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            var userOrder = await _orderRepository
                                            .Find(o => o.Id == orderId && o.UserId == userId)
                                            .Include(o => o.OrderItems)
                                            .Include(os => os.OrderStatus)
                                            .FirstOrDefaultAsync();

            if (userOrder == null || userOrder.OrderStatusId != OrderStatusConstants.Active)
            {
                throw new KeyNotFoundException("order was not found");
            }

            foreach (var product in products)
            {
                var orderItem = userOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == product.ProductId);

                if (orderItem != null)
                {
                    orderItem.ItemQuantity -= product.quantity;

                    if (orderItem.ItemQuantity <= 0)
                    {
                        userOrder.OrderItems.Remove(orderItem);
                    }
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

        public async Task CheckoutOrder(Guid userId, Guid orderId)
        {
            var order = await _orderRepository
                                    .GetAll()
                                    .Where(q => q.Id == orderId && q.UserId == userId)
                                    .Include(oi => oi.OrderItems)
                                    .ThenInclude(o => o.Product)
                                    .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new KeyNotFoundException("permission to order denied");
            }

            order.OrderStatusId = OrderStatusConstants.Confirmed;

            foreach (var item in order.OrderItems)
            {
                if (item.Product.Quantity < item.ItemQuantity)
                {
                    throw new Exception("Unfortunately we don't have enough products in storage");
                }
                else
                {
                    item.Product.Quantity -= item.ItemQuantity;
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}