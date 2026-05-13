using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Combo;
using FastFood.Models.Product;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class ComboController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ComboController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
        {
            try
            {
                var url = $"api/combo{(string.IsNullOrEmpty(searchTerm) ? "" : $"?searchTerm={searchTerm}")}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var combos = await response.Content.ReadFromJsonAsync<List<ComboModel>>();
                    
                    if (combos == null) combos = new List<ComboModel>();

                    // Pagination (10 items per page)
                    int pageSize = 10;
                    int totalItems = combos.Count;
                    int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                    combos = combos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.SearchTerm = searchTerm;
                    
                    return View(combos);
                }
            }
            catch { }

            return View(new List<ComboModel>());
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/combo/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var combo = await response.Content.ReadFromJsonAsync<ComboDetailModel>();
                    return View(combo);
                }
            }
            catch { }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareViewBag();
            return View(new ComboModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComboModel model, List<int> productIds, List<int> quantities)
        {
            if (!ModelState.IsValid)
            {
                await PrepareViewBag();
                return View(model);
            }

            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await SaveImage(model.ImageFile);
                }

                var items = new List<object>();
                for (int i = 0; i < productIds.Count; i++)
                {
                    if (productIds[i] > 0)
                        items.Add(new { productId = productIds[i], quantity = quantities[i] });
                }

                var request = new
                {
                    name = model.Name,
                    description = model.Description,
                    imageUrl = model.ImageUrl,
                    comboPrice = model.ComboPrice,
                    comboType = model.ComboType,
                    isActive = model.IsActive,
                    items = items
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(request, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/combo", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm combo thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi endpoint api: " + ex.Message);
            }

            await PrepareViewBag();
            ModelState.AddModelError("", "Không thể thêm combo.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/combo/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var combo = await response.Content.ReadFromJsonAsync<ComboDetailModel>();
                    await PrepareViewBag();
                    return View(combo);
                }
            }
            catch { }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ComboDetailModel model, List<int> productIds, List<int> quantities)
        {
            if (!ModelState.IsValid)
            {
                await PrepareViewBag();
                return View(model);
            }

            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await SaveImage(model.ImageFile);
                }

                var items = new List<object>();
                for (int i = 0; i < productIds.Count; i++)
                {
                    if (productIds[i] > 0)
                        items.Add(new { productId = productIds[i], quantity = quantities[i] });
                }

                var request = new
                {
                    name = model.Name,
                    description = model.Description,
                    imageUrl = model.ImageUrl,
                    comboPrice = model.ComboPrice,
                    comboType = model.ComboType,
                    isActive = model.IsActive,
                    items = items
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(request, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/combo/{model.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật combo thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi endpoint api: " + ex.Message);
            }

            await PrepareViewBag();
            ModelState.AddModelError("", "Không thể cập nhật combo.");
            return View(model);
        }

        private async Task PrepareViewBag()
        {
            var productsResponse = await _httpClient.GetAsync("api/product");
            if (productsResponse.IsSuccessStatusCode)
            {
                ViewBag.Products = await productsResponse.Content.ReadFromJsonAsync<List<ProductModel>>();
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string comboPath = Path.Combine(wwwRootPath, @"images\products"); // Keep combo images in products folder for simplicity or create separate

            if (!Directory.Exists(comboPath))
            {
                Directory.CreateDirectory(comboPath);
            }

            using (var fileStream = new FileStream(Path.Combine(comboPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return @"/images/products/" + fileName;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/combo/{id}/toggle", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đã cập nhật trạng thái combo!";
                }
                else
                {
                    TempData["Error"] = "Không thể cập nhật trạng thái combo.";
                }
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/combo/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Combo deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete combo.";
                }
            }
            catch
            {
                TempData["Error"] = "An error occurred while deleting the combo.";
            }

            return RedirectToAction("Index");
        }
    }
}
