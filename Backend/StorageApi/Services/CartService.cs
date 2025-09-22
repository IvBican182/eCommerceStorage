
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StorageApi.Data;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> CreateCart(Guid userId, List<(Guid productId, int quantity)> products)
        {
            var user = await _context.Users.FindAsync(userId);

            var cart = new Cart();

            if (user == null)
            {
                throw new KeyNotFoundException("user not found");
            }

            cart.UserId = userId;

            foreach (var item in products)
            {
                var cartItem = new CartItem
                {
                    ProductId = item.productId,
                    CartId = cart.Id,
                    Quantity = item.quantity


                };

                cart.CartItems.Add(cartItem);
            }

            await _context.Carts.AddAsync(cart);

            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> GetUserCart(Guid userId)
        {
            var userCart = await _context.Carts.Include(ci => ci.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (userCart == null)
            {
                throw new KeyNotFoundException("user cart not found");
            }
            return userCart;
        }

        public async Task<Cart> AddItemToCart(Guid id, List<(Guid productId, int quantity)> products)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            foreach (var item in products)
            {
                var existingCartItem = cart.CartItems.FirstOrDefault(
                    ci => ci.ProductId == item.productId && ci.CartId == cart.Id);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += item.quantity;
                }

                var cartItem = new CartItem
                {
                    ProductId = item.productId,
                    CartId = cart.Id,
                    Quantity = item.quantity


                };

                cart.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return cart;



        }

        public async Task<Cart> RemoveItemFromCart(Guid id, List<(Guid productId, int quantity)> products)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            foreach (var product in products)
            {
                var existingCartItem = cart.CartItems.FirstOrDefault(
                    ci => ci.ProductId == product.productId && ci.CartId == cart.Id);

                if (existingCartItem == null)
                {
                    throw new KeyNotFoundException("Product isnt in the cart");

                }

                existingCartItem.Quantity -= product.quantity;

                if (existingCartItem.Quantity <= 0)
                {
                    _context.CartItems.Remove(existingCartItem);
                }


            }

            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteCart(Guid cartId, Guid userId)
        {
            var userCart = await _context.Carts
                                    .Include(ci => ci.CartItems)
                                    .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == cartId);

            if (userCart == null)
            {
                throw new KeyNotFoundException("User cart not found");
            }
            _context.Carts.Remove(userCart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}