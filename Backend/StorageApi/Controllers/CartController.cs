
using Microsoft.AspNetCore.Mvc;
using StorageApi.Interfaces;
using StorageApi.Models;
using StorageApi.ModelsDTO;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetUserCart(Guid userId)
        {
            try
            {
                var userCart = await _cartService.GetUserCart(userId);
                return userCart;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<Cart>> CreateCart(Guid userId, List<AddRemoveCartItemDto> products)
        {
            try
            {
                var newCart = await _cartService.CreateCart(userId, products);
                return newCart;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{cartId}/add-items")]
        public async Task<ActionResult<Cart>> AddItemToCart(Guid cartId, List<AddRemoveCartItemDto> products)
        {
            try
            {
                var cart = await _cartService.AddItemToCart(cartId, products);
                return cart;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{cartId}/remove-items")]
        public async Task<ActionResult<Cart>> RemoveItemFromCart(Guid cartId, List<AddRemoveCartItemDto> products)
        {
            try
            {
                var cart = await _cartService.RemoveItemFromCart(cartId, products);
                return cart;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{cartId}/{userId}")]
        public async Task<IActionResult> DeleteCart(Guid cartId, Guid userId)
        {
            try
            {
                var cart = await _cartService.DeleteCart(cartId, userId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}