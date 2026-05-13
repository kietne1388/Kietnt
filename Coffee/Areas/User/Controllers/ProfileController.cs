using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.User;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var response = await _httpClient.GetAsync($"api/auth/user/{userId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserProfileModel>();
                    if (user != null)
                    {
                        // Mock or fetch actual stats
                        var ordersResp = await _httpClient.GetAsync($"api/order/user/{userId.Value}");
                        if (ordersResp.IsSuccessStatusCode)
                        {
                            var orders = await ordersResp.Content.ReadFromJsonAsync<List<object>>();
                            user.TotalOrders = orders?.Count ?? 0;
                        }

                        var vouchersResp = await _httpClient.GetAsync($"api/voucher/user/{userId.Value}");
                        if (vouchersResp.IsSuccessStatusCode)
                        {
                            var vouchers = await vouchersResp.Content.ReadFromJsonAsync<List<object>>();
                            user.VoucherCount = vouchers?.Count ?? 0;
                        }

                        user.Points = user.TotalOrders * 10; // Mock points logic
                    }
                    return View(user);
                }
            }
            catch { }

            return View(new UserProfileModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var response = await _httpClient.GetAsync($"api/auth/user/{userId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserProfileModel>();
                    var model = new UpdateProfileModel
                    {
                        FullName = user?.FullName ?? "",
                        Email = user?.Email ?? "",
                        PhoneNumber = user?.PhoneNumber ?? "",
                        Avatar = user?.Avatar
                    };
                    return View(model);
                }
            }
            catch { }

            return View(new UpdateProfileModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProfileModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var json = JsonSerializer.Serialize(new
                {
                    userId = userId.Value,
                    fullName = model.FullName,
                    email = model.Email,
                    phoneNumber = model.PhoneNumber,
                    avatar = model.Avatar,
                    address = model.Address
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/auth/user/{userId.Value}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Profile updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch { }

            ModelState.AddModelError("", "Failed to update profile.");
            return View(model);
        }
    }
}
