using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Order;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index(string status = "")
        {
            try
            {
                var url = string.IsNullOrEmpty(status) ? "api/order" : $"api/order?status={status}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
                    ViewBag.StatusFilter = status;
                    return View(orders ?? new List<OrderModel>());
                }
            }
            catch { }

            return View(new List<OrderModel>());
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/order/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<OrderDetailModel>();
                    return View(order);
                }
            }
            catch { }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusModel model)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(new { status = model.Status }, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/order/{model.OrderId}/status", content);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    if (response.IsSuccessStatusCode)
                        return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
                    return Json(new { success = false, message = "Không thể cập nhật trạng thái." });
                }

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Cập nhật trạng thái thành công!";
                else
                    TempData["Error"] = "Không thể cập nhật trạng thái.";

                return RedirectToAction("Detail", new { id = model.OrderId });
            }
            catch
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Có lỗi xảy ra." });
                
                TempData["Error"] = "Có lỗi xảy ra.";
                return RedirectToAction("Detail", new { id = model.OrderId });
            }
        }
    }

    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
