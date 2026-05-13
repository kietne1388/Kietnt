using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Comment;
using System.Text.Json;
using System.Text;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class CommentController : Controller
    {
        private readonly HttpClient _httpClient;

        public CommentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/comment");
                if (response.IsSuccessStatusCode)
                {
                    var comments = await response.Content.ReadFromJsonAsync<List<CommentModel>>();
                    return View(comments ?? new List<CommentModel>());
                }
            }
            catch { }

            return View(new List<CommentModel>());
        }

        [HttpPost]
        public async Task<IActionResult> ToggleHide(int id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/comment/{id}/hide", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Comment visibility updated!";
                }
            }
            catch { }

            return RedirectToAction("Index");
        }
    }
}
