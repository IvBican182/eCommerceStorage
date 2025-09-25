
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
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
        public async Task<ActionResult<CartDTO>> CreateCart(Guid userId,[FromBody] List<AddRemoveCartItemDto> products)
        {
            try
            {
                var cart = await _cartService.CreateCart(userId, products);
                var cartdto = _mapper.Map<CartDTO>(cart);
                return cartdto;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{cartId}/add-items")]
        public async Task<ActionResult<CartDTO>> AddItemToCart(Guid cartId,[FromBody] List<AddRemoveCartItemDto> products)
        {
            try
            {
                var cart = await _cartService.AddItemToCart(cartId, products);
                var cartdto = _mapper.Map<CartDTO>(cart);
                return cartdto;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{cartId}/remove-items")]
        public async Task<ActionResult<CartDTO>> RemoveItemFromCart(Guid cartId,[FromBody] List<AddRemoveCartItemDto> products)
        {
            try
            {
                var cart = await _cartService.RemoveItemFromCart(cartId, products);
                var cartdto = _mapper.Map<CartDTO>(cart);
                return cartdto;
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