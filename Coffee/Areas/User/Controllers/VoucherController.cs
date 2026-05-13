using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Voucher;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
    public class VoucherController : Controller
    {
        private readonly HttpClient _httpClient;

        public VoucherController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/voucher/active");
                if (response.IsSuccessStatusCode)
                {
                    var vouchers = await response.Content.ReadFromJsonAsync<List<VoucherModel>>();
                    return View(vouchers ?? new List<VoucherModel>());
                }
            }
            catch { }

            return View(new List<VoucherModel>());
        }

        public async Task<IActionResult> MyVouchers()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var response = await _httpClient.GetAsync($"api/voucher/user/{userId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var vouchers = await response.Content.ReadFromJsonAsync<List<UserVoucherModel>>();
                    return View(vouchers ?? new List<UserVoucherModel>());
                }
            }
            catch { }

            return View(new List<UserVoucherModel>());
        }
    }
}
