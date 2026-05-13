using Microsoft.AspNetCore.Mvc;
using FastFood.Application.Interfaces;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportApiController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IAuthService _authService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ReportApiController(
            IReportService reportService,
            IAuthService authService,
            IProductService productService,
            IOrderService orderService)
        {
            _reportService = reportService;
            _authService = authService;
            _productService = productService;
            _orderService = orderService;
        }

        [HttpGet("revenue/daily")]
        public async Task<IActionResult> GetDailyRevenue([FromQuery] DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;
            var revenue = await _reportService.GetDailyRevenueAsync(targetDate);
            return Ok(new { date = targetDate, revenue });
        }

        [HttpGet("revenue/monthly")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int? year, [FromQuery] int? month)
        {
            var targetYear = year ?? DateTime.Now.Year;
            var targetMonth = month ?? DateTime.Now.Month;
            var revenue = await _reportService.GetMonthlyRevenueAsync(targetYear, targetMonth);
            return Ok(new { year = targetYear, month = targetMonth, revenue });
        }

        [HttpGet("revenue/yearly")]
        public async Task<IActionResult> GetYearlyRevenue([FromQuery] int? year)
        {
            var targetYear = year ?? DateTime.Now.Year;
            var revenue = await _reportService.GetYearlyRevenueAsync(targetYear);
            return Ok(new { year = targetYear, revenue });
        }

        // Dashboard calls this as 'order-statistics'
        [HttpGet("order-statistics")]
        public async Task<IActionResult> GetOrderStatusStatistics()
        {
            var statistics = await _reportService.GetOrderStatusStatisticsAsync();
            return Ok(statistics);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int count = 10)
        {
            var products = await _reportService.GetTopSellingProductsAsync(count);
            return Ok(products.Select(p => new { p.ProductName, p.ImageUrl, p.TotalSold, p.Revenue }));
        }

        // New: Dashboard summary endpoint
        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var todayRevenue = await _reportService.GetDailyRevenueAsync(DateTime.Today);
            var monthRevenue = await _reportService.GetMonthlyRevenueAsync(DateTime.Now.Year, DateTime.Now.Month);
            var yearRevenue = await _reportService.GetYearlyRevenueAsync(DateTime.Now.Year);
            var orderStats = await _reportService.GetOrderStatusStatisticsAsync();
            var topProducts = await _reportService.GetTopSellingProductsAsync(5);
            var users = await _authService.GetAllUsersAsync();
            var products = await _productService.GetAllProductsAsync();

            return Ok(new
            {
                todayRevenue,
                monthRevenue,
                yearRevenue,
                totalOrders = orderStats.Values.Sum(),
                pendingOrders = orderStats.ContainsKey("Pending") ? orderStats["Pending"] : 0,
                totalUsers = users.Count(),
                totalProducts = products.Count(),
                topProducts = topProducts.Select(p => new { p.ProductName, p.ImageUrl, p.TotalSold, p.Revenue }),
                orderStatistics = orderStats
            });
        }

        [HttpGet("custom-range")]
        public async Task<IActionResult> GetCustomRangeReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var revenueByDate = await _reportService.GetRevenueByDateRangeAsync(startDate, endDate);
            var topProducts = await _reportService.GetTopSellingItemsByDateRangeAsync(startDate, endDate, 5);

            return Ok(new
            {
                revenueByDate,
                topProducts = topProducts.Select(p => new { p.ProductName, p.ImageUrl, p.TotalSold, p.Revenue })
            });
        }

        [HttpGet("item-stats")]
        public async Task<IActionResult> GetItemStats()
        {
            var stats = await _reportService.GetItemStatsAsync();
            return Ok(stats);
        }
    }
}
