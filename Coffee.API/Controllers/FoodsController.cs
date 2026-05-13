using FastFood.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/foods")]
    [Authorize]
    public class FoodsController : ControllerBase
    {
        private readonly IProductService _productService;

        public FoodsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var (items, totalCount) = await _productService.GetPagedProductsAsync(page, size);
            return Ok(new { items, totalCount });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound(new { message = "Food not found" });
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFoodRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _productService.CreateProductAsync(
                request.Name, request.Description, request.Price, request.ImageUrl, request.CategoryId);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFoodRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _productService.UpdateProductAsync(
                id, request.Name, request.Description, request.Price, request.ImageUrl, request.CategoryId);
            if (product == null) return NotFound(new { message = "Food not found" });
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success) return NotFound(new { message = "Food not found" });
            return Ok(new { message = "Food deleted successfully" });
        }
    }

    public record CreateFoodRequest(string Name, string Description, decimal Price, string ImageUrl, int CategoryId);
    public record UpdateFoodRequest(string Name, string Description, decimal Price, string ImageUrl, int CategoryId);
}
