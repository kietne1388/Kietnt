using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Comment;
using System.Text.Json;
using System.Text;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class CommentController : Controller
    {
        private readonly HttpClient _httpClient;

        public CommentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Vui lòng đăng nhập để bình luận.";
                if (model.ComboId.HasValue)
                    return RedirectToAction("Detail", "Combo", new { id = model.ComboId });
                else
                    return RedirectToAction("Detail", "Product", new { id = model.ProductId });
            }

            try
            {
                var request = new
                {
                    UserId = userId.Value,
                    ProductId = model.ProductId,
                    ComboId = model.ComboId,
                    Content = model.Content,
                    Rating = model.Rating,
                    ParentId = model.ParentId
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/comment", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đã gửi đánh giá của bạn!";
                }
                else
                {
                    TempData["Error"] = "Không thể gửi đánh giá. Vui lòng thử lại.";
                }
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra.";
            }

            if (model.ComboId.HasValue)
                return RedirectToAction("Detail", "Combo", new { id = model.ComboId });
            else
                return RedirectToAction("Detail", "Product", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int commentId, int? productId, int? comboId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["Error"] = "Vui lòng đăng nhập.";
                if (comboId.HasValue)
                    return RedirectToAction("Detail", "Combo", new { id = comboId });
                else
                    return RedirectToAction("Detail", "Product", new { id = productId });
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"api/comment/{commentId}?userId={userId.Value}");
                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Đã xóa bình luận.";
                else
                    TempData["Error"] = "Không thể xóa bình luận này.";
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra.";
            }

            if (comboId.HasValue)
                return RedirectToAction("Detail", "Combo", new { id = comboId });
            else
                return RedirectToAction("Detail", "Product", new { id = productId });
        }
    }
}
