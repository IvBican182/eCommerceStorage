

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Core.Interfaces;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderservice;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderservice = orderService;
            _mapper = mapper;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<OrderDTO>> CreateOrder(Guid cartId, Guid userId)
        {
            try
            {
                var order = await _orderservice.CreateOrder(cartId, userId);
                var orderDto = _mapper.Map<OrderDTO>(order);
                return orderDto;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{orderId}/add-order-item")]
        public async Task<ActionResult<OrderDTO>> AddOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            try
            {
                var order = await _orderservice.AddOrderItem(orderId, userId, products);
                var orderDto = _mapper.Map<OrderDTO>(order);
                return orderDto;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("{orderId}/remove-order-item")]
        public async Task<ActionResult<OrderDTO>> RemoveOrderItem(Guid orderId, Guid userId, List<OrderItemDto> products)
        {
            try
            {
                var order = await _orderservice.RemoveOrderItem(orderId, userId, products);
                var orderDto = _mapper.Map<OrderDTO>(order);
                return orderDto;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId, Guid userId)
        {
            try
            {
                await _orderservice.DeleteOrder(orderId, userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutOrder(Guid userId, Guid orderId)
        {
            try
            {
                await _orderservice.CheckoutOrder(userId, orderId);
                return Ok(new { message = "Order succesful" });
            }
            catch (InvalidOperationException er)
            {
                return BadRequest(new { error = er.Message });
            }
        }
    }    
}