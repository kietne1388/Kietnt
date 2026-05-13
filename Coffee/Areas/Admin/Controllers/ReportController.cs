using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Revenue()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/report/revenue/monthly");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<Dictionary<string, decimal>>();
                    return View(data ?? new Dictionary<string, decimal>());
                }
            }
            catch { }

            return View(new Dictionary<string, decimal>());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomRangeReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/report/custom-range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<object>();
                    return Json(data);
                }
                return BadRequest("API returned error: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> TopProducts(int limit = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/report/top-products?limit={limit}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();
                    return View(data ?? new List<Dictionary<string, object>>());
                }
            }
            catch { }

            return View(new List<Dictionary<string, object>>());
        }
    }
}
