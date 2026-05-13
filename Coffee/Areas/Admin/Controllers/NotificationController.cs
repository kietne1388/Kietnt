using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using System.Text.Json;
using System.Text;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class NotificationController : Controller
    {
        private readonly HttpClient _httpClient;

        public NotificationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendBroadcast(string title, string message, string? url)
        {
            try
            {
                var request = new
                {
                    UserId = (int?)null,
                    Type = "Broadcast",
                    Title = title,
                    Message = message,
                    Url = url
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var content = new StringContent(JsonSerializer.Serialize(request, options), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/notification/create", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Notification broadcasted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to broadcast notification.";
                }
            }
            catch
            {
                TempData["Error"] = "An error occurred.";
            }

            return RedirectToAction("Index");
        }
    }
}
