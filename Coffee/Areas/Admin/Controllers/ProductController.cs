using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Admin.Product;
using FastFood.Models.Category;
using FastFood.Models.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int page = 1)
        {
            try
            {
                var url = $"api/product?searchTerm={searchTerm}&categoryId={categoryId}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
                    
                    if (products == null) products = new List<ProductModel>();

                    // Pagination (10 items per page)
                    int pageSize = 10;
                    int totalItems = products.Count;
                    int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                    products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.SearchTerm = searchTerm;
                    ViewBag.CategoryId = categoryId;
                    
                    return View(products);
                }
            }
            catch { }

            return View(new List<ProductModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCategoriesAsync();
            return View(new CreateProductModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await GetCategoriesAsync();
                return View(model);
            }

            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await SaveImage(model.ImageFile);
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/product", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi endpoint api: " + ex.Message);
            }

            ViewBag.Categories = await GetCategoriesAsync();
            ModelState.AddModelError("", "Không thể thêm sản phẩm.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ViewBag.Categories = await GetCategoriesAsync();
                var response = await _httpClient.GetAsync($"api/product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var product = await response.Content.ReadFromJsonAsync<ProductModel>();
                    if (product != null)
                    {
                        var model = new UpdateProductModel
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            ImageUrl = product.ImageUrl,
                            CategoryId = product.CategoryId,
                            IsActive = product.IsActive
                        };
                        return View(model);
                    }
                }
            }
            catch { }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await GetCategoriesAsync();
                return View(model);
            }

            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await SaveImage(model.ImageFile);
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/product/{model.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi endpoint api: " + ex.Message);
            }

            ViewBag.Categories = await GetCategoriesAsync();
            ModelState.AddModelError("", "Không thể cập nhật sản phẩm.");
            return View(model);
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images\products");

            if (!Directory.Exists(productPath))
            {
                Directory.CreateDirectory(productPath);
            }

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return @"/images/products/" + fileName;
        }

        private async Task<List<SelectListItem>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/category");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<CategoryModel>>();
                    return categories?.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList() ?? new List<SelectListItem>();
                }
            }
            catch { }

            return new List<SelectListItem>();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/product/{id}/toggle", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đã cập nhật trạng thái sản phẩm!";
                }
                else
                {
                    TempData["Error"] = "Không thể cập nhật trạng thái sản phẩm.";
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
                var response = await _httpClient.DeleteAsync($"api/product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa sản phẩm thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể xóa sản phẩm.";
                }
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Comments(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/comment/product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var comments = await response.Content.ReadFromJsonAsync<List<FastFood.Models.Comment.CommentModel>>();
                    ViewBag.ProductId = id;
                    
                    // Get product info for header
                    var prodResponse = await _httpClient.GetAsync($"api/product/{id}");
                    if (prodResponse.IsSuccessStatusCode)
                    {
                        var product = await prodResponse.Content.ReadFromJsonAsync<ProductModel>();
                        ViewBag.ProductName = product?.Name;
                    }

                    return View(comments ?? new List<FastFood.Models.Comment.CommentModel>());
                }
            }
            catch { }

            return View(new List<FastFood.Models.Comment.CommentModel>());
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCommentHide(int id, int productId)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/comment/{id}/hide", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đã cập nhật trạng thái bình luận!";
                }
            }
            catch { }

            return RedirectToAction("Comments", new { id = productId });
        }
    }
}
