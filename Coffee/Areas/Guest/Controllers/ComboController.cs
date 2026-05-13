using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Combo;
using System.Text.Json;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class ComboController : Controller
    {
        private readonly HttpClient _httpClient;

        public ComboController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        // ============================
        // DANH SÁCH COMBO + FILTER
        // ============================
        public async Task<IActionResult> Index(
            string? comboType,
            decimal? minPrice,
            decimal? maxPrice,
            string? sort,
            int page = 1)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/combo/active");
                if (!response.IsSuccessStatusCode)
                    return View(new List<ComboModel>());

                var combos = await response.Content
                    .ReadFromJsonAsync<List<ComboModel>>()
                    ?? new List<ComboModel>();

                // Filter theo ComboType
                if (!string.IsNullOrEmpty(comboType))
                {
                    var actualComboType = comboType switch
                    {
                        "1" => "1 Người",
                        "2" => "2 Người",
                        "3" => "3+ Người",
                        "4" => "Gia Đình",
                        _ => comboType
                    };

                    combos = combos
                        .Where(c =>
                            !string.IsNullOrEmpty(c.ComboType) &&
                            c.ComboType.Equals(actualComboType, StringComparison.OrdinalIgnoreCase)
                        )
                        .ToList();
                }

                // Filter theo giá
                if (minPrice.HasValue)
                    combos = combos.Where(c => c.ComboPrice >= minPrice.Value).ToList();

                if (maxPrice.HasValue)
                    combos = combos.Where(c => c.ComboPrice <= maxPrice.Value).ToList();

                // Sort
                if (!string.IsNullOrEmpty(sort))
                {
                    combos = sort switch
                    {
                        "price-asc" => combos.OrderBy(c => c.ComboPrice).ToList(),
                        "price-desc" => combos.OrderByDescending(c => c.ComboPrice).ToList(),
                        "discount" => combos.OrderByDescending(c => c.OriginalPrice - c.ComboPrice).ToList(),
                        _ => combos
                    };
                }

                // Pagination (4 items per page)
                int pageSize = 4;
                int totalItems = combos.Count;
                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                combos = combos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.ComboType = comboType;
                ViewBag.MinPrice = minPrice;
                ViewBag.MaxPrice = maxPrice;
                ViewBag.Sort = sort;

                return View(combos);
            }
            catch
            {
                return View(new List<ComboModel>());
            }
        }

        // ============================
        // CHI TIẾT COMBO
        // ============================
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/combo/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound();

                // API trả về ComboDto có cấu trúc: ComboItems[].Product.Name/ImageUrl/Price
                // Cần map thủ công sang ComboDetailModel
                var json = await response.Content.ReadAsStringAsync();
                var apiCombo = JsonSerializer.Deserialize<ComboApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiCombo == null)
                    return NotFound();

                var model = new ComboDetailModel
                {
                    Id = apiCombo.Id,
                    Name = apiCombo.Name,
                    Description = apiCombo.Description ?? "",
                    ComboPrice = apiCombo.ComboPrice,
                    OriginalPrice = apiCombo.OriginalPrice,
                    SavedAmount = apiCombo.OriginalPrice - apiCombo.ComboPrice,
                    ImageUrl = apiCombo.ImageUrl,
                    IsActive = apiCombo.IsActive,
                    Items = apiCombo.ComboItems?.Select(ci => new ComboItemModel
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product?.Name ?? "Sản phẩm",
                        ProductImageUrl = ci.Product?.ImageUrl ?? "/images/default-food.jpg",
                        Quantity = ci.Quantity,
                        ProductPrice = ci.Product?.Price ?? 0
                    }).ToList() ?? new List<ComboItemModel>()
                };

                var commentsResponse = await _httpClient.GetAsync($"api/comment/combo/{id}");
                if (commentsResponse.IsSuccessStatusCode)
                {
                    model.Comments = await commentsResponse.Content.ReadFromJsonAsync<List<FastFood.Application.DTOs.CommentDto>>() ?? new();
                    model.CommentCount = model.Comments.Count;
                    model.AverageRating = model.Comments.Any() ? model.Comments.Average(c => c.Rating) : 0;
                }

                return View(model);
            }
            catch
            {
                return NotFound();
            }
        }
    }

    // DTO classes to deserialize the API response correctly
    public class ComboApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ComboPrice { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public List<ComboItemApiResponse>? ComboItems { get; set; }
    }

    public class ComboItemApiResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductApiResponse? Product { get; set; }
    }

    public class ProductApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
