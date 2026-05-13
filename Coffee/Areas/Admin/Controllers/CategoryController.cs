using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Category;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/category/paged?page={page}&size=10");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CategoryPagedResult>();
                    if (result != null)
                    {
                        ViewBag.CurrentPage = page;
                        ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / 10.0);
                        return View(result.Items);
                    }
                }
            }
            catch { }

            return View(new List<CategoryModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCategoryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/category", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm danh mục thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi kết nối API: " + ex.Message);
            }

            ModelState.AddModelError("", "Không thể thêm danh mục.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/category/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var category = await response.Content.ReadFromJsonAsync<CategoryModel>();
                    if (category != null)
                    {
                        var model = new UpdateCategoryModel
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Description = category.Description
                        };
                        return View(model);
                    }
                }
            }
            catch { }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/category/{model.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật danh mục thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi kết nối API: " + ex.Message);
            }

            ModelState.AddModelError("", "Không thể cập nhật danh mục.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/category/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa danh mục thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể xóa danh mục.";
                }
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra.";
            }

            return RedirectToAction("Index");
        }

        // ===== AJAX JSON ACTIONS FOR MODAL =====
        [HttpPost]
        public async Task<IActionResult> CreateJson([FromBody] CreateCategoryModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Name))
                return Json(new { success = false, message = "Tên danh mục không được để trống." });

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/category", content);

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true });

                return Json(new { success = false, message = "Không thể thêm danh mục. Vui lòng thử lại." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi kết nối: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditJson([FromBody] UpdateCategoryModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Name))
                return Json(new { success = false, message = "Tên danh mục không được để trống." });

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/category/{model.Id}", content);

                if (response.IsSuccessStatusCode)
                    return Json(new { success = true });

                return Json(new { success = false, message = "Không thể cập nhật danh mục. Vui lòng thử lại." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi kết nối: " + ex.Message });
            }
        }

        private class CategoryPagedResult
        {
            public List<CategoryModel> Items { get; set; } = new();
            public int TotalCount { get; set; }
        }
    }
}
