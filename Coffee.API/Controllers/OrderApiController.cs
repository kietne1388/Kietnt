using FastFood.Application.Interfaces;
using FastFood.Application.DTOs; // Added namespace
using Microsoft.AspNetCore.Mvc;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var order = await _orderService.CreateOrderAsync(
                request.UserId,
                request.Address,
                request.Items
            );

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status = null)
        {
            var orders = await _orderService.GetAllOrdersAsync(status);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var success = await _orderService.UpdateOrderStatusAsync(id, request.Status);

            if (!success)
                return NotFound(new { message = "Order not found" });

            return Ok(new { message = "Order status updated successfully" });
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var success = await _orderService.CancelOrderAsync(id);

            if (!success)
                return BadRequest(new { message = "Cannot cancel this order" });

            return Ok(new { message = "Order cancelled successfully" });
        }
    }



    public record CreateOrderRequest(int UserId, string Address, List<CartItemDto> Items);
    
    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}
