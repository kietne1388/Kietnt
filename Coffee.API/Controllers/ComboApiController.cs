using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using FastFood.Application.Interfaces;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/combo")]
    public class ComboApiController : ControllerBase
    {
        private readonly IComboService _comboService;

        public ComboApiController(IComboService comboService)
        {
            _comboService = comboService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null)
        {
            var combos = await _comboService.GetAllCombosAsync(searchTerm);
            return Ok(combos);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var combos = await _comboService.GetActiveCombosAsync();
            return Ok(combos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combo = await _comboService.GetComboByIdAsync(id);

            if (combo == null)
                return NotFound(new { message = "Combo not found" });

            return Ok(combo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComboRequest request)
        {
            var combo = await _comboService.CreateComboAsync(
                request.Name,
                request.Description,
                request.ComboPrice,
                request.ComboType,
                request.ImageUrl,
                request.Items.Select(i => (i.ProductId, i.Quantity)).ToList()
            );

            return CreatedAtAction(nameof(GetById), new { id = combo.Id }, combo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateComboRequest request)
        {
            var success = await _comboService.UpdateComboAsync(
                id,
                request.Name,
                request.Description,
                request.ComboPrice,
                request.ComboType,
                request.ImageUrl,
                request.Items.Select(i => (i.ProductId, i.Quantity)).ToList()
            );

            if (!success)
                return NotFound(new { message = "Combo not found" });

            return Ok(new { message = "Combo updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _comboService.DeleteComboAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Combo deleted successfully" });
        }

        [HttpPost("{id}/toggle")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var success = await _comboService.ToggleActiveAsync(id);
            if (!success) return NotFound(new { message = "Combo not found" });
            return Ok(new { message = "Combo visibility toggled successfully" });
        }
    }

    public class ComboItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public record CreateComboRequest(string Name, string? Description, decimal ComboPrice, string? ComboType, string? ImageUrl, bool IsActive, List<ComboItemRequest> Items);
    public record UpdateComboRequest(string Name, string? Description, decimal ComboPrice, string? ComboType, string? ImageUrl, bool IsActive, List<ComboItemRequest> Items);
}
