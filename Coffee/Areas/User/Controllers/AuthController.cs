using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.User;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var json = JsonSerializer.Serialize(new { userId = userId.Value, oldPassword = model.OldPassword, newPassword = model.NewPassword });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/auth/change-password", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Password changed successfully!";
                    return RedirectToAction("Index", "Profile");
                }
            }
            catch { }

            ModelState.AddModelError("", "Failed to change password. Please check your old password.");
            return View(model);
        }
    }
}
