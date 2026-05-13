using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Comment;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
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
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid comment data.";
                return RedirectToAction("Detail", "Product", new { area = "Guest", id = model.ProductId });
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Register", new { area = "Guest" });
            }

            try
            {
                var json = JsonSerializer.Serialize(new
                {
                    userId = userId.Value,
                    productId = model.ProductId,
                    content = model.Content,
                    rating = model.Rating
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/comment", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Comment added successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to add comment.";
                }
            }
            catch
            {
                TempData["Error"] = "An error occurred while adding comment.";
            }

            return RedirectToAction("Detail", "Product", new { area = "Guest", id = model.ProductId });
        }

        public async Task<IActionResult> MyComments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Register", new { area = "Guest" });
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/comment/user/{userId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var comments = await response.Content.ReadFromJsonAsync<List<CommentModel>>();
                    return View(comments ?? new List<CommentModel>());
                }
            }
            catch { }

            return View(new List<CommentModel>());
        }
    }
}
