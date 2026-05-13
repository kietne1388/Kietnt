using FastFood.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/orderitems")]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var (items, totalCount) = await _orderItemService.GetPagedOrderItemsAsync(page, size);
            return Ok(new { items, totalCount });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var item = await _orderItemService.CreateOrderItemAsync(request.FoodId, request.Quantity);
                return Ok(item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderItemService.DeleteOrderItemAsync(id);
            if (!success) return NotFound(new { message = "OrderItem not found" });
            return Ok(new { message = "OrderItem deleted successfully" });
        }
    }

    public record CreateOrderItemRequest(int FoodId, int Quantity);
}
