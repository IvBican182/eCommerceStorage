
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StorageApi.Data;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;
using StorageApi.Core.Constants;
using System.Threading.Tasks;

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

            if (user == null)
            {
                throw new KeyNotFoundException("user not found");
            }

            var activeUserCart = await _cartRepository
                                            .GetAll()
                                            .Include(a => a.CartItems)
                                            .FirstOrDefaultAsync(c => c.UserId == userId && c.CartStatus.Name == "Active");

            if (activeUserCart != null)
            {
                foreach (var item in products)
                {
                    var existingCartItem = activeUserCart.CartItems.Where(ci => ci.ProductId == item.ProductId).SingleOrDefault();
                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += item.quantity;
                    }
                    else
                    {
                        var cartItem = new CartItem
                        {
                            ProductId = item.ProductId,
                            CartId = activeUserCart.Id,
                            Quantity = item.quantity,
                        };

                        activeUserCart.CartItems.Add(cartItem);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return activeUserCart;

            }
            else
            {
                var newCart = new Cart();

                newCart.UserId = userId;
                newCart.CartStatusId = CartStatusConstants.Active;

                foreach (var item in products)
                {
                    var cartItem = new CartItem
                    {
                        ProductId = item.ProductId,
                        CartId = newCart.Id,
                        Quantity = item.quantity
                    };

                    newCart.CartItems.Add(cartItem);
                }

                _cartRepository.Add(newCart);
                await _unitOfWork.SaveChangesAsync();
                return newCart;

            }
        }

        public async Task<Cart> GetUserCart(Guid userId)
        {
            var userCart = await _cartRepository
                                        .GetAll()
                                        .Include(ci => ci.CartItems)
                                        .FirstOrDefaultAsync(c => c.UserId == userId && c.CartStatus.Name == "Active");

            if (userCart == null)
            {
                throw new KeyNotFoundException("user cart not found");
            }
            return userCart;
        }

        public async Task<Cart> AddItemToCart(Guid userId, AddRemoveCartItemDto product)
        {
            var cart = await GetUserCart(userId);

            if (cart == null)
            {
                throw new KeyNotFoundException("cart not found");
            }

            var productItem = cart.CartItems.Where(q => q.ProductId == product.ProductId).SingleOrDefault();

            if (productItem == null)
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.ProductId,
                    Quantity = product.quantity
                };
                cart.CartItems.Add(newCartItem);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                productItem.Quantity += product.quantity;
                await _unitOfWork.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> RemoveItemFromCart(Guid userId, List<AddRemoveCartItemDto> products)
        {
            var cart = await GetUserCart(userId);

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

        public async Task<DeleteCartResponseDto> DeleteCart(Guid cartId, Guid userId)
        {
            var userCart = await _cartRepository.GetAll()
                                                .Where(c => c.Id == cartId)
                                                .Include(ci => ci.CartItems)
                                                .FirstOrDefaultAsync();

            if (userCart == null)
            {
                return new DeleteCartResponseDto { Success = false, Error = "user cart was not found" };
            }
            _cartRepository.Remove(userCart);
            await _unitOfWork.SaveChangesAsync();
            return new DeleteCartResponseDto { Success = true };
        }
    }
}