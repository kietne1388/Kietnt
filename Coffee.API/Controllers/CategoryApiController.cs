using FastFood.Application.Interfaces;
using FastFood.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _categoryService.GetPagedCategoriesAsync(page, size);
            return Ok(new { items = result.Items, totalCount = result.TotalCount });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var success = await _categoryService.CreateCategoryAsync(request.Name, request.Description);
            if (!success) return BadRequest("Could not create category.");
            return Ok(new { message = "Category created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest request)
        {
            var success = await _categoryService.UpdateCategoryAsync(id, request.Name, request.Description);
            if (!success) return NotFound();
            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Category deleted successfully" });
        }
    }

}
