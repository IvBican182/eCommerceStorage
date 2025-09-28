
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
        public async Task<ActionResult<CartDTO>> GetUserCart(Guid userId)
        {
            var userCart = await _cartService.GetUserCart(userId);
            var userCartDto = _mapper.Map<CartDTO>(userCart);

            return Ok(userCartDto);

        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<CartDTO>> CreateCart(Guid userId,[FromBody] List<AddRemoveCartItemDto> products)
        {
           
            var cart = await _cartService.CreateCart(userId, products);
            var cartdto = _mapper.Map<CartDTO>(cart);

            if (cartdto == null)
            {
                return BadRequest(new { error = "error occured while creating a cart, invalid inputs" });
            }

            return Ok(cartdto);
            
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
                return NotFound(new { error = ex.Message });
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
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{cartId}/{userId}")]
        public async Task<ActionResult<DeleteCartResponseDto>> DeleteCart(Guid cartId, Guid userId)
        {
            var deleteCartResponse = await _cartService.DeleteCart(cartId, userId);

            if (!deleteCartResponse.Success)
            { 
                return NotFound(new { error = deleteCartResponse.Error });
            }

            return NoContent();
        }
    }
}