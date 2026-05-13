using FastFood.Application.Interfaces;
using FastFood.Application.DTOs;
using FastFood.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var (items, totalCount) = await _categoryService.GetPagedCategoriesAsync(page, size);
            return Ok(new { items, totalCount });
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _categoryService.CreateCategoryAsync(request.Name, request.Description);
            if (!result) return BadRequest(new { message = "Không thể tạo danh mục" });
            return Ok(new { message = "Tạo danh mục thành công" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _categoryService.UpdateCategoryAsync(id, request.Name, request.Description);
            if (!result) return NotFound(new { message = "Danh mục không tồn tại" });
            return Ok(new { message = "Cập nhật danh mục thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result) return NotFound(new { message = "Danh mục không tồn tại" });
            return Ok(new { message = "Xóa danh mục thành công" });
        }
    }

}
