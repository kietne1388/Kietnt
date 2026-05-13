using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Order;
using FastFood.Models.Cart;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var response = await _httpClient.GetAsync($"api/order/user/{userId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
                    return View(orders ?? new List<OrderModel>());
                }
            }
            catch { }

            return View(new List<OrderModel>());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var response = await _httpClient.GetAsync($"api/order/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<OrderDetailModel>();
                    if (order != null)
                    {
                        return View(order);
                    }
                }
            }
            catch { }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CheckoutModel model)
        {
            if (!ModelState.IsValid) return View("Checkout", model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var orderData = new
                {
                    userId = userId.Value,
                    items = model.Items.Select(i => new { productId = i.ProductId, quantity = i.Quantity }).ToList(),
                    address = model.Address,
                    voucherCode = model.VoucherCode
                };

                var json = JsonSerializer.Serialize(orderData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/order", content);

                if (response.IsSuccessStatusCode)
                {
                    // Clear cart
                    HttpContext.Session.Remove("Cart");
                    TempData["Success"] = "Order placed successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch { }

            ModelState.AddModelError("", "Failed to create order. Please try again.");
            return View("Checkout", model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Register", new { area = "Guest" });

            try
            {
                var response = await _httpClient.PostAsync($"api/order/{id}/cancel", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Order cancelled successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to cancel order.";
                }
            }
            catch
            {
                TempData["Error"] = "An error occurred while cancelling the order.";
            }

            return RedirectToAction("Detail", new { id });
        }
    }
}
