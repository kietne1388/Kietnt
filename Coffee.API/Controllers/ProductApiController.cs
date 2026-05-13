using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using FastFood.Application.Interfaces;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null, [FromQuery] int? categoryId = null)
        {
            var products = await _productService.GetAllProductsAsync(searchTerm, categoryId);
            return Ok(products);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = await _productService.CreateProductAsync(
                request.Name,
                request.Description,
                request.Price,
                request.ImageUrl,
                request.CategoryId
            );

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            var product = await _productService.UpdateProductAsync(
                id,
                request.Name,
                request.Description,
                request.Price,
                request.ImageUrl,
                request.CategoryId
            );

            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (!success)
                return NotFound(new { message = "Product not found" });

            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpPost("{id}/toggle")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var success = await _productService.ToggleActiveAsync(id);

            if (!success)
                return NotFound(new { message = "Product not found" });

            return Ok(new { message = "Product visibility toggled successfully" });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterByPrice([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var products = await _productService.FilterByPriceAsync(min, max);
            return Ok(products);
        }
    }

    public record CreateProductRequest(string Name, string Description, decimal Price, string ImageUrl, int CategoryId);
    public record UpdateProductRequest(string Name, string Description, decimal Price, string ImageUrl, int CategoryId);
}
