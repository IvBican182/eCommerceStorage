
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Interfaces;
using StorageApi.Models;
using StorageApi.ModelsDTO;

namespace StorageApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Cart cart, Guid userId)
        {
            var userCart = await _context.Carts.FindAsync(cart.Id);

            if (userCart == null)
            {
                throw new KeyNotFoundException("order not found");
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

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order;

        }
        public async Task<Order> AddOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            var userOrder = await _context.Orders.Include(oi => oi.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (userOrder == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            foreach (var product in products)
            {
                var orderItem = userOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == product.ProductId);

                orderItem.ItemQuantity += product.quantity;

            }

            await _context.SaveChangesAsync();

            return userOrder;
            

        }

        public async Task<Order> RemoveOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            var userOrder = await _context.Orders.Include(oi => oi.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (userOrder == null)
            {
                throw new KeyNotFoundException("cart not found");
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

            await _context.SaveChangesAsync();

            return userOrder;


        }

        public async Task<bool> DeleteOrder(Guid orderId, Guid userId)
        {
            var userOrder = await _context.Orders.Include(oi => oi.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (userOrder == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            _context.Orders.Remove(userOrder);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}