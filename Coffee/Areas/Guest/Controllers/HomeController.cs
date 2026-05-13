using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Product;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/product/active");
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
                    return View(products ?? new List<ProductModel>());
                }
            }
            catch { }
            
            return View(new List<ProductModel>());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> GetWelcomeVouchers(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/voucher/user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }
                return StatusCode((int)response.StatusCode);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
