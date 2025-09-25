
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StorageApi.Data;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;

namespace StorageApi.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cartRepository = _unitOfWork.GetRepository<Cart>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<Cart> CreateCart(Guid userId, List<AddRemoveCartItemDto> products)
        {
            var user = await _userRepository.GetByIdAsync(userId);

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
                    ProductId = item.ProductId,
                    CartId = cart.Id,
                    Quantity = item.quantity


                };

                cart.CartItems.Add(cartItem);
            }

            _cartRepository.Add(cart);

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> GetUserCart(Guid userId)
        {
            var userCart = await _cartRepository
                                        .GetAll()
                                        .Where(c => c.UserId == userId)
                                        .Include(ci => ci.CartItems)
                                        .FirstOrDefaultAsync();

            if (userCart == null)
            {
                throw new KeyNotFoundException("user cart not found");
            }
            return userCart;
        }

        public async Task<Cart> AddItemToCart(Guid id, List<AddRemoveCartItemDto> products)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            foreach (var item in products)
            {
                var existingCartItem = cart.CartItems.FirstOrDefault(
                    ci => ci.ProductId == item.ProductId && ci.CartId == cart.Id);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += item.quantity;
                }

                var cartItem = new CartItem
                {
                    ProductId = item.ProductId,
                    CartId = cart.Id,
                    Quantity = item.quantity


                };

                cart.CartItems.Add(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
            return cart;



        }

        public async Task<Cart> RemoveItemFromCart(Guid id, List<AddRemoveCartItemDto> products)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            foreach (var product in products)
            {
                var existingCartItem = cart.CartItems.FirstOrDefault(
                    ci => ci.ProductId == product.ProductId && ci.CartId == cart.Id);

                if (existingCartItem == null)
                {
                    throw new KeyNotFoundException("Product isnt in the cart");

                }

                existingCartItem.Quantity -= product.quantity;

                if (existingCartItem.Quantity <= 0)
                {
                    cart.CartItems.Remove(existingCartItem);
                }


            }

            await _unitOfWork.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteCart(Guid cartId, Guid userId)
        {
            var userCart = await _cartRepository.GetAll()
                                                .Where(c => c.Id == cartId)
                                                .Include(ci => ci.CartItems)
                                                .FirstOrDefaultAsync();

            if (userCart == null)
            {
                throw new KeyNotFoundException("User cart not found");
            }
            _cartRepository.Remove(userCart);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}