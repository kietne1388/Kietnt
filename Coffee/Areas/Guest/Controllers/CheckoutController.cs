using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Cart;
using FastFood.Models.Checkout;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using System.Text.Json;

namespace FastFood.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IVoucherService _voucherService;
        private readonly IEmailService _emailService;
        private readonly HttpClient _httpClient;
        private const string CartSessionKey = "CartItems";

        public CheckoutController(
            IHttpClientFactory httpClientFactory,
            IOrderService orderService,
            IVoucherService voucherService,
            IEmailService emailService)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
            _orderService = orderService;
            _voucherService = voucherService;
            _emailService = emailService;
        }

        // GET: Hiển thị trang thanh toán
        [HttpGet]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth", new { area = "Guest" });
            }

            var cart = GetCartFromSession();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                Items = cart,
                SubTotal = cart.Sum(x => x.Total),
                ShippingFee = 20000
            };

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(model);
        }

        // POST: Xử lý từ trang Cart chuyển sang
        [HttpPost]
        public IActionResult Index([FromForm] List<int> SelectedItems)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth", new { area = "Guest" });
            }

            var cart = GetCartFromSession();
            var checkoutItems = cart;

            if (SelectedItems != null && SelectedItems.Any())
            {
                checkoutItems = cart.Where(x =>
                    (x.ProductId > 0 && SelectedItems.Contains(x.ProductId)) ||
                    (x.ComboId != null)
                ).ToList();
            }

            if (!checkoutItems.Any())
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm để thanh toán";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                Items = checkoutItems,
                SubTotal = checkoutItems.Sum(x => x.Total),
                ShippingFee = 20000
            };

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(model);
        }

        // POST: Xử lý ĐẶT HÀNG
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            // 1. Kiểm tra đăng nhập bằng SESSION (không dùng Claims)
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth", new { area = "Guest" });
            }

            // 2. Lấy giỏ hàng hiện tại
            var cart = GetCartFromSession();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }

            try
            {
                // 3. Map CartItems -> CartItemDtos
                var orderItems = cart.Select(x => new CartItemDto
                {
                    ProductId = x.ProductId,
                    ComboId = x.ComboId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList();

                decimal discountAmount = 0;
                var sessionDiscount = HttpContext.Session.GetString("AppliedDiscount");
                if (!string.IsNullOrEmpty(model.VoucherCode) && !string.IsNullOrEmpty(sessionDiscount) && decimal.TryParse(sessionDiscount, out decimal parsedDiscount))
                {
                    discountAmount = parsedDiscount;
                }

                var orderDto = await _orderService.CreateOrderAsync(userId.Value, model.Address ?? "", orderItems, discountAmount);

                if (orderDto != null)
                {
                    // 4. Áp dụng voucher giảm giá nếu có
                    if (!string.IsNullOrEmpty(model.VoucherCode))
                    {
                        try
                        {
                            await _voucherService.ApplyVoucherAsync(model.VoucherCode);
                        }
                        catch { /* Voucher đã được áp dụng trên UI, chỉ cần deduct quantity */ }
                    }

                    // 5. Gửi email xác nhận đơn hàng
                    try
                    {
                        var userEmail = HttpContext.Session.GetString("UserEmail");
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            await _emailService.SendOrderConfirmationAsync(orderDto, userEmail);
                        }
                    }
                    catch { /* Email thất bại không ảnh hưởng đơn hàng */ }

                    // 6. Xóa giỏ hàng
                    HttpContext.Session.Remove(CartSessionKey);
                    HttpContext.Session.Remove("CartCount");

                    TempData["Success"] = "Đặt hàng thành công!";

                    // 7. Chuyển hướng theo phương thức thanh toán
                    if (model.PaymentMethod == "Bank")
                    {
                        return RedirectToAction("BankTransfer", new { orderId = orderDto.Id });
                    }
                    else if (model.PaymentMethod == "MoMo")
                    {
                        return RedirectToAction("MoMoPayment", new { orderId = orderDto.Id });
                    }

                    return RedirectToAction("OrderSuccess");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi đặt hàng: " + ex.Message;
            }

            // Nếu lỗi, load lại trang checkout
            model.Items = cart;
            model.SubTotal = cart.Sum(x => x.Total);
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("Index", model);
        }

        public IActionResult OrderSuccess() => View();

        public IActionResult BankTransfer(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        public IActionResult MoMoPayment(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        // Áp dụng Voucher - Dùng VoucherService trực tiếp
        [HttpPost]
        public async Task<IActionResult> ApplyVoucher([FromBody] ApplyVoucherRequest request)
        {
            try
            {
                var voucher = await _voucherService.GetVoucherByCodeAsync(request.Code);
                if (voucher != null)
                {
                    var discount = await _voucherService.CalculateDiscountAsync(request.Code, request.OrderAmount);

                    if (discount > 0)
                    {
                        // Lưu voucher code vào session để dùng khi PlaceOrder
                        HttpContext.Session.SetString("AppliedVoucherCode", request.Code);
                        HttpContext.Session.SetString("AppliedDiscount", discount.ToString());

                        return Json(new { success = true, discount = discount, message = $"Áp dụng mã thành công! Giảm {discount:N0}đ" });
                    }
                }
                return Json(new { success = false, message = "Mã không hợp lệ hoặc đã hết hạn" });
            }
            catch
            {
                return Json(new { success = false, message = "Lỗi hệ thống" });
            }
        }

        // --- Helpers ---
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }
    }

    public class ApplyVoucherRequest
    {
        public string Code { get; set; } = "";
        public decimal OrderAmount { get; set; }
    }

    public class VoucherInfo
    {
        public string Code { get; set; } = "";
        public decimal DiscountAmount { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsActive { get; set; }
    }
}