using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Cart;
using System.Text.Json;
// Thêm namespace chứa Model Combo của bạn nếu cần
// using FastFood.Models.Combo; 

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string CartSessionKey = "CartItems";

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
        }

        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        // 1. GIỮ NGUYÊN AddToCart (Cho món lẻ)
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            // ... (Code cũ của bạn giữ nguyên, không cần sửa) ...
            // Chỉ lưu ý: Khi new CartItem, ComboId sẽ tự động là null
             try
            {
                var response = await _httpClient.GetAsync($"api/product/{request.ProductId}");
                if (!response.IsSuccessStatusCode) return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                var product = await response.Content.ReadFromJsonAsync<ProductInfo>();
                if (product == null) return Json(new { success = false, message = "Lỗi data" });

                var cart = GetCartFromSession();
                // Sửa logic tìm kiếm: Phải là món lẻ (ComboId == null)
                var existingItem = cart.FirstOrDefault(x => x.ProductId == request.ProductId && x.ComboId == null);

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Quantity = request.Quantity,
                        ComboId = null // Xác nhận đây là món lẻ
                    });
                }
                SaveCartToSession(cart);
                UpdateCartCount(cart);
                return Json(new { success = true, message = "Đã thêm món!", cartCount = cart.Sum(x => x.Quantity) });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // 2. THÊM MỚI: Hàm thêm Combo vào giỏ
        [HttpPost]
        public async Task<IActionResult> AddComboToCart([FromBody] AddComboRequest request)
        {
            try
            {
                // Gọi API lấy thông tin Combo
                var response = await _httpClient.GetAsync($"api/combo/{request.ComboId}");
                if (!response.IsSuccessStatusCode) return Json(new { success = false, message = "Combo không tồn tại" });

                // Bạn cần class ComboInfo tương ứng (xem ở cuối bài)
                var combo = await response.Content.ReadFromJsonAsync<ComboInfo>();
                if (combo == null) return Json(new { success = false, message = "Lỗi dữ liệu Combo" });
                
                var cart = GetCartFromSession();
                // Tìm xem trong giỏ đã có Combo này chưa
                var existingItem = cart.FirstOrDefault(x => x.ComboId == request.ComboId);

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                    // Logic merge notes:
                    if (request.Notes != null && request.Notes.Any())
                    {
                        foreach (var note in request.Notes)
                        {
                            if (existingItem.ComboItemNotes.ContainsKey(note.Key))
                            {
                                existingItem.ComboItemNotes[note.Key] += $"; {note.Value}";
                            }
                            else
                            {
                                existingItem.ComboItemNotes[note.Key] = note.Value;
                            }
                        }
                    }
                }
                else
                {
                    // Add nguyên cục Combo vào giỏ
                    cart.Add(new CartItem
                    {
                        ProductId = 0, // Combo thì không có ProductId, để 0
                        ComboId = combo.Id, // Lưu ID Combo vào đây
                        ProductName = combo.Name, // Tên Combo (VD: Combo Vui Vẻ)
                        Price = combo.ComboPrice, // Giá Combo
                        ImageUrl = combo.ImageUrl ?? "combo-default.jpg", 
                        Quantity = request.Quantity,
                        ComboItemNotes = request.Notes ?? new Dictionary<int, string>() // Lưu notes
                    });
                }

                SaveCartToSession(cart);
                UpdateCartCount(cart);

                return Json(new { success = true, message = "Đã thêm Combo!", cartCount = cart.Sum(x => x.Quantity) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // 3. SỬA: UpdateQuantity phải nhận cả ComboId để biết đường update
        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var cart = GetCartFromSession();
            
            // Logic tìm item quan trọng: Check cả ProductId lẫn ComboId
            var item = cart.FirstOrDefault(x => 
                (request.ComboId.HasValue && x.ComboId == request.ComboId) || 
                (!request.ComboId.HasValue && x.ProductId == request.ProductId && x.ComboId == null)
            );

            if (item != null)
            {
                if (request.Quantity <= 0) cart.Remove(item);
                else item.Quantity = request.Quantity;
                
                SaveCartToSession(cart);
                UpdateCartCount(cart);
            }

            return Json(new { success = true, cartCount = cart.Sum(x => x.Quantity), total = cart.Sum(x => x.Price * x.Quantity) });
        }

        // 4. SỬA: RemoveItem cũng tương tự
        [HttpPost]
        public IActionResult RemoveItem([FromBody] RemoveItemRequest request)
        {
            var cart = GetCartFromSession();
            // Logic tìm item để xóa
            var item = cart.FirstOrDefault(x => 
                (request.ComboId.HasValue && x.ComboId == request.ComboId) || 
                (!request.ComboId.HasValue && x.ProductId == request.ProductId && x.ComboId == null)
            );

            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);
                UpdateCartCount(cart);
            }

            return Json(new { success = true, cartCount = cart.Sum(x => x.Quantity) });
        }

        [HttpGet]
        public IActionResult GetCart()
        {
            var cart = GetCartFromSession();
            return Json(new { 
                count = cart.Sum(x => x.Quantity),
                total = cart.Sum(x => x.Price * x.Quantity)
            });
        }

        // ... Các hàm GetCart, Helper giữ nguyên ...
        
        // --- CÁC HÀM HELPER KHÁC GIỮ NGUYÊN ---
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            return cart ?? new List<CartItem>();
        }
        private void SaveCartToSession(List<CartItem> cart) => HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        private void UpdateCartCount(List<CartItem> cart) => HttpContext.Session.SetInt32("CartCount", cart.Sum(x => x.Quantity));
    }

    // --- CẬP NHẬT CÁC CLASS DTO (Ở cuối file Controller) ---

    public class AddToCartRequest { public int ProductId { get; set; } public int Quantity { get; set; } = 1; }
    
    // Class mới cho Combo
    public class AddComboRequest 
    { 
        public int ComboId { get; set; } 
        public int Quantity { get; set; } = 1;
        public Dictionary<int, string>? Notes { get; set; } // Nhận notes từ client
    }

    // Cập nhật Update/Remove để hỗ trợ Combo
    public class UpdateQuantityRequest { 
        public int ProductId { get; set; } 
        public int? ComboId { get; set; } // Thêm dòng này
        public int Quantity { get; set; } 
    }
    public class RemoveItemRequest { 
        public int ProductId { get; set; } 
        public int? ComboId { get; set; } // Thêm dòng này
    }

    public class ProductInfo { public int Id { get; set; } public string Name { get; set; } = ""; public decimal Price { get; set; } public string ImageUrl { get; set; } = ""; }
    
    // Class để hứng dữ liệu API Combo
    public class ComboInfo { 
        public int Id { get; set; } 
        public string Name { get; set; } = ""; 
        public decimal ComboPrice { get; set; } // Chú ý tên property này phải khớp JSON API trả về
        public string? ImageUrl { get; set; } 
    }
}