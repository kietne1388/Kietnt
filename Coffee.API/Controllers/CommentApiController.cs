using Microsoft.AspNetCore.Mvc;
using FastFood.Application.Interfaces;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentApiController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var comments = await _commentService.GetCommentsByProductIdAsync(productId);
            return Ok(comments);
        }

        [HttpGet("combo/{comboId}")]
        public async Task<IActionResult> GetByComboId(int comboId)
        {
            var comments = await _commentService.GetCommentsByComboIdAsync(comboId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            var comment = await _commentService.CreateCommentAsync(
                request.UserId,
                request.ProductId,
                request.ComboId,
                request.Content,
                request.Rating,
                request.ParentId
            );
            return Ok(comment);
        }

        [HttpPut("{id}/hide")]
        public async Task<IActionResult> Hide(int id)
        {
            var success = await _commentService.HideCommentAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Comment hidden" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            if (userId <= 0) return Unauthorized(new { message = "Unauthorized" });
            var success = await _commentService.DeleteCommentAsync(id, userId);
            if (!success) return BadRequest(new { message = "Không thể xóa bình luận này." });
            return Ok(new { message = "Đã xóa bình luận." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }
    }

    public class CreateCommentRequest
    {
        public int UserId { get; set; }
        public int? ProductId { get; set; }
        public int? ComboId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int? ParentId { get; set; }
    }
}
