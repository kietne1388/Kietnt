using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Admin.Voucher;
using FastFood.Models.Voucher;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
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
                var response = await _httpClient.GetAsync("api/voucher");
                if (response.IsSuccessStatusCode)
                {
                    var vouchers = await response.Content.ReadFromJsonAsync<List<VoucherModel>>();
                    return View(vouchers ?? new List<VoucherModel>());
                }
            }
            catch { }

            return View(new List<VoucherModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVoucherModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/voucher", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm voucher thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi endpoint api: " + ex.Message);
            }

            ModelState.AddModelError("", "Không thể thêm voucher.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/voucher");
                if (response.IsSuccessStatusCode)
                {
                    var vouchers = await response.Content.ReadFromJsonAsync<List<VoucherModel>>();
                    var voucher = vouchers?.FirstOrDefault(v => v.Id == id);
                    if (voucher != null)
                    {
                        var model = new UpdateVoucherModel
                        {
                            Code = voucher.Code,
                            Description = voucher.Description,
                            DiscountAmount = voucher.DiscountAmount,
                            DiscountPercent = voucher.DiscountPercent,
                            ExpiredAt = voucher.ExpiredAt,
                            Quantity = voucher.Quantity
                        };
                        ViewBag.Id = id;
                        return View(model);
                    }
                }
            }
            catch { }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateVoucherModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/voucher/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật voucher thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi endpoint api: " + ex.Message);
            }

            ModelState.AddModelError("", "Không thể cập nhật voucher.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/voucher/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Voucher deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete voucher.";
                }
            }
            catch
            {
                TempData["Error"] = "An error occurred while deleting the voucher.";
            }

            return RedirectToAction("Index");
        }
    }
}
