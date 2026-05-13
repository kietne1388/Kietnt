using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.User;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/auth/users");
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<UserProfileModel>>();
                    return View(users ?? new List<UserProfileModel>());
                }
            }
            catch { }

            return View(new List<UserProfileModel>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTier(int id, string tier)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/auth/update-tier/{id}", new { Tier = tier });
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Membership tier updated successfully!";
                }
            }
            catch { }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/auth/toggle-active/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "User status updated successfully!";
                }
            }
            catch { }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/auth/user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserProfileModel>();
                    return View(user);
                }
            }
            catch { }

            return NotFound();
        }
    }
}
