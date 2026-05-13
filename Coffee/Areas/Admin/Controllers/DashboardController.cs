using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Admin.Dashboard;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            try
            {
                // Gọi API dashboard-summary (1 lần gọi, lấy hết data)
                var response = await _httpClient.GetAsync("api/report/dashboard-summary");
                if (response.IsSuccessStatusCode)
                {
                    var summary = await response.Content.ReadFromJsonAsync<DashboardSummaryResponse>();
                    if (summary != null)
                    {
                        model.Today = new RevenueTodayModel { Revenue = summary.TodayRevenue };
                        model.Month = new RevenueMonthModel { Revenue = summary.MonthRevenue };
                        model.Year = new RevenueYearModel { Revenue = summary.YearRevenue };
                        model.TotalOrders = summary.TotalOrders;
                        model.PendingOrders = summary.PendingOrders;
                        model.TotalUsers = summary.TotalUsers;
                        model.TotalProducts = summary.TotalProducts;
                        model.TopProducts = summary.TopProducts ?? new List<TopProductModel>();
                    }
                }

                // Fetch recent orders separately
                var ordersResponse = await _httpClient.GetAsync("api/order");
                if (ordersResponse.IsSuccessStatusCode)
                {
                    var orders = await ordersResponse.Content.ReadFromJsonAsync<List<RecentOrderModel>>();
                    model.RecentOrders = orders?.OrderByDescending(o => o.CreatedAt).Take(10).ToList()
                                         ?? new List<RecentOrderModel>();
                }

                // Fetch item stats for charts
                var statsResponse = await _httpClient.GetAsync("api/report/item-stats");
                if (statsResponse.IsSuccessStatusCode)
                {
                    var stats = await statsResponse.Content.ReadFromJsonAsync<ItemStatsDto>();
                    ViewBag.ProductStats = stats?.Products ?? new List<ItemStatDto>();
                    ViewBag.ComboStats = stats?.Combos ?? new List<ItemStatDto>();
                }
                else
                {
                    ViewBag.ProductStats = new List<ItemStatDto>();
                    ViewBag.ComboStats = new List<ItemStatDto>();
                }
            }
            catch { }

            return View(model);
        }
    }

    // Response model for dashboard-summary API
    public class DashboardSummaryResponse
    {
        public decimal TodayRevenue { get; set; }
        public decimal MonthRevenue { get; set; }
        public decimal YearRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public List<TopProductModel> TopProducts { get; set; } = new();
    }

    // DTO for item stats chart
    public class ItemStatDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int TotalSold { get; set; }
        public bool IsActive { get; set; }
    }

    public class ItemStatsDto
    {
        public List<ItemStatDto> Products { get; set; } = new();
        public List<ItemStatDto> Combos { get; set; } = new();
    }
}
