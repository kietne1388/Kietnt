using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Product;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public async Task<IActionResult> Index(int? categoryId, decimal? minPrice, decimal? maxPrice, string? sort, int page = 1)
        {
            try
            {
                var catsResponse = await _httpClient.GetAsync("api/category");
                if (catsResponse.IsSuccessStatusCode)
                {
                    ViewBag.Categories = await catsResponse.Content.ReadFromJsonAsync<List<FastFood.Models.Category.CategoryModel>>();
                }

                // Fetch all active products first, we will paginate after filtering
                var queryString = $"api/product/active";
                
                // Pass params to API in case it supports them
                if (categoryId.HasValue) queryString += $"?categoryId={categoryId}";
                if (minPrice.HasValue) queryString += (queryString.Contains("?") ? "&" : "?") + $"minPrice={minPrice}";
                if (maxPrice.HasValue) queryString += (queryString.Contains("?") ? "&" : "?") + $"maxPrice={maxPrice}";
                if (!string.IsNullOrEmpty(sort)) queryString += (queryString.Contains("?") ? "&" : "?") + $"sort={sort}";

                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
                    
                    if (products != null)
                    {
                        if (categoryId.HasValue)
                        {
                             products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
                        }

                        if (minPrice.HasValue)
                            products = products.Where(p => p.Price >= minPrice.Value).ToList();
                        
                        if (maxPrice.HasValue)
                            products = products.Where(p => p.Price <= maxPrice.Value).ToList();

                        // 2. Client-side Sorting
                        if (!string.IsNullOrEmpty(sort))
                        {
                            products = sort switch
                            {
                                "price-asc" => products.OrderBy(p => p.Price).ToList(),
                                "price-desc" => products.OrderByDescending(p => p.Price).ToList(),
                                "name" => products.OrderBy(p => p.Name).ToList(),
                                _ => products
                            };
                        }
                    }

                    // 3. Pagination (6 items per page)
                    int pageSize = 6;
                    int totalItems = products?.Count ?? 0;
                    int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                    if (products != null)
                    {
                        products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }

                    // Set ViewBag
                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentCategory = categoryId;
                    ViewBag.MinPrice = minPrice;
                    ViewBag.MaxPrice = maxPrice;
                    ViewBag.Sort = sort;

                    return View(products ?? new List<ProductModel>());
                }
            }
            catch { }
            
            return View(new List<ProductModel>());
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var product = await response.Content.ReadFromJsonAsync<ProductDetailModel>();
                    
                    if (product != null)
                    {
                        var commentsResponse = await _httpClient.GetAsync($"api/comment/product/{id}");
                        if (commentsResponse.IsSuccessStatusCode)
                        {
                            var dtos = await commentsResponse.Content.ReadFromJsonAsync<List<FastFood.Application.DTOs.CommentDto>>() ?? new();
                            product.Comments = dtos.Select(c => new FastFood.Models.Comment.CommentModel 
                            { 
                                Id = c.Id,
                                UserId = c.UserId,
                                UserName = c.UserName,
                                ProductId = c.ProductId ?? 0,
                                ProductName = c.ProductName ?? "",
                                Content = c.Content,
                                Rating = c.Rating,
                                IsHidden = c.IsHidden,
                                ParentId = c.ParentId,
                                CreatedAt = c.CreatedAt,
                                Replies = c.Replies?.Select(r => new FastFood.Models.Comment.CommentModel
                                {
                                    Id = r.Id,
                                    UserId = r.UserId,
                                    UserName = r.UserName,
                                    Content = r.Content,
                                    CreatedAt = r.CreatedAt
                                }).ToList() ?? new()
                            }).ToList();
                            product.CommentCount = product.Comments.Count;
                            product.AverageRating = product.Comments.Any() ? product.Comments.Average(c => c.Rating) : 0;
                        }
                    }

                    return View(product);
                }
            }
            catch { }
            
            return NotFound();
        }
    }
}
