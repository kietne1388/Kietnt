using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Product;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class SearchController : Controller
    {
        private readonly HttpClient _httpClient;

        public SearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return View(new List<ProductModel>());
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/product?search={keyword}");
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
                    ViewBag.Keyword = keyword;
                    return View(products ?? new List<ProductModel>());
                }
            }
            catch { }

            ViewBag.Keyword = keyword;
            return View(new List<ProductModel>());
        }
    }
}
