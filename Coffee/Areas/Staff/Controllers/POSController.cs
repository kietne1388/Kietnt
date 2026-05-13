using Microsoft.AspNetCore.Mvc;
using FastFood.Models.Product;
using FastFood.Models;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FastFood.Application.Interfaces;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Entities;

namespace FastFood.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class POSController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherService _voucherService;
        private readonly AppDbContext _context;

        public POSController(IHttpClientFactory httpClientFactory, 
                             IOrderService orderService,
                             IUserRepository userRepository,
                             IVoucherService voucherService,
                             AppDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient("FastFoodAPI");
            _orderService = orderService;
            _userRepository = userRepository;
            _voucherService = voucherService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Auth", new { area = "Guest" });

            // Ensure they are an employee or admin
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin" && role != "Staff")
            {
                // In this dummy system, we'll let anyone try it out if we make them an employee in memory
                // Let's create a test fallback
            }

            // Sync or Find Employee profile from DB
            var email = HttpContext.Session.GetString("UserEmail");
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (dbUser == null) return RedirectToAction("Login", "Auth", new { area = "Guest" });

            var employee = new EmployeeModel 
            { 
                Id = dbUser.Id, 
                HoTen = dbUser.FullName, 
                ChucVu = dbUser.Role, 
                Email = dbUser.Email,
                TrangThai = dbUser.IsActive
            };

            ViewBag.Employee = employee;

            // Check if clocked in from DB
            var attendanceEntry = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == dbUser.Id && !a.ClockOutTime.HasValue);
            
            AttendanceModel? attendance = null;
            if (attendanceEntry != null)
            {
                attendance = new AttendanceModel
                {
                    Id = attendanceEntry.Id,
                    EmployeeId = attendanceEntry.EmployeeId,
                    ClockInTime = attendanceEntry.ClockInTime,
                    HourlyRate = attendanceEntry.HourlyRate
                };
            }
            ViewBag.CurrentAttendance = attendance;

            // Load Categories
            try
            {
                var catsResponse = await _httpClient.GetAsync("api/category");
                if (catsResponse.IsSuccessStatusCode)
                {
                    ViewBag.Categories = await catsResponse.Content.ReadFromJsonAsync<List<FastFood.Models.Category.CategoryModel>>();
                }
            }
            catch { }

            return View();
        }

        // AJAX: Load Products
        [HttpGet]
        public async Task<IActionResult> LoadProducts(int? categoryId)
        {
            try
            {
                var url = "api/product/active";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>() ?? new();
                    if (categoryId.HasValue && categoryId > 0)
                    {
                        products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
                    }
                    return Json(products);
                }
            }
            catch { }
            return Json(new List<ProductModel>());
        }

        // AJAX: Load Combos
        [HttpGet]
        public async Task<IActionResult> LoadCombos()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/combo/active");
                if (response.IsSuccessStatusCode)
                {
                    var combos = await response.Content.ReadFromJsonAsync<List<FastFood.Models.Combo.ComboModel>>() ?? new();
                    // Map to a unified ProductModel-like object for the POS
                    var result = combos.Select(c => new {
                        id = c.Id,
                        name = c.Name,
                        price = c.ComboPrice,
                        imageUrl = c.ImageUrl,
                        categoryId = 0,
                        isCombo = true,
                        description = c.Description
                    });
                    return Json(result);
                }
            }
            catch { }
            return Json(new object[0]);
        }

        // AJAX: Get product rankings by sales
        [HttpGet]
        public async Task<IActionResult> GetRankings()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/report/item-stats");
                if (response.IsSuccessStatusCode)
                {
                    var stats = await response.Content.ReadFromJsonAsync<FastFood.Application.Interfaces.ItemStatsResult>();
                    if (stats != null)
                    {
                        var productRanks = stats.Products
                            .Take(5)
                            .Select((p, i) => new { id = p.Id, rank = i + 1, totalSold = p.TotalSold });
                        var comboRanks = stats.Combos
                            .Take(5)
                            .Select((c, i) => new { id = c.Id, rank = i + 1, totalSold = c.TotalSold });
                        return Json(new { products = productRanks, combos = comboRanks });
                    }
                }
            }
            catch { }
            return Json(new { products = new object[0], combos = new object[0] });
        }

        // POST: Clock In
        [HttpPost]
        public async Task<IActionResult> ClockIn(int employeeId)
        {
            var existing = await _context.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == employeeId && !a.ClockOutTime.HasValue);
            if (existing != null) return Json(new { success = false, message = "Đã điểm danh vào ca rồi!" });

            var user = await _context.Users.FindAsync(employeeId);
            if (user == null) return Json(new { success = false, message = "Không tìm thấy nhân viên." });

            var log = new Attendance
            {
                EmployeeId = user.Id,
                ClockInTime = DateTime.Now,
                HourlyRate = 30000 // default
            };
            
            _context.Attendances.Add(log);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Vào ca thành công!", time = log.ClockInTime.ToString("HH:mm") });
        }

        // POST: Clock Out
        [HttpPost]
        public async Task<IActionResult> ClockOut(int employeeId)
        {
            var log = await _context.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == employeeId && !a.ClockOutTime.HasValue);
            if (log == null) return Json(new { success = false, message = "Chưa điểm danh vào ca!" });

            log.ClockOutTime = DateTime.Now;
            await _context.SaveChangesAsync();

            var elapsed = log.ClockOutTime.Value - log.ClockInTime;
            var earned = log.TotalSalary;

            return Json(new { 
                success = true, 
                message = "Hết ca thành công!", 
                time = log.ClockOutTime.Value.ToString("HH:mm"),
                elapsed = $"{(int)elapsed.TotalHours}h {elapsed.Minutes}m",
                earned = earned.ToString("N0") + " đ"
            });
        }

        // AJAX: Check for online orders (Notifications)
        [HttpGet]
        public async Task<IActionResult> GetOnlineOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync("Pending");
            return Json(orders.Take(5)); // Return latest 5 pending orders
        }

        // AJAX: Get customer tier by phone
        [HttpGet]
        public async Task<IActionResult> GetCustomerInfo(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return Json(null);
            var user = await _userRepository.GetByPhoneAsync(phone);
            if (user == null) return Json(null);
            
            return Json(new { 
                fullName = user.FullName, 
                tier = user.MembershipTier,
                discountPercent = user.MembershipTier == "VIP" ? 10 : (user.MembershipTier == "Loyal" ? 7 : 0)
            });
        }

        // AJAX: Validate Voucher
        [HttpGet]
        public async Task<IActionResult> ValidateVoucher(string code)
        {
            if (string.IsNullOrEmpty(code)) return Json(new { success = false, message = "Vui lòng nhập mã." });
            
            var voucher = await _voucherService.GetVoucherByCodeAsync(code);
            if (voucher == null || !voucher.IsActive || voucher.ExpiredAt < DateTime.Now)
                return Json(new { success = false, message = "Mã không hợp lệ hoặc đã hết hạn." });

            return Json(new { 
                success = true, 
                discountAmount = voucher.DiscountAmount,
                discountPercent = voucher.DiscountPercent ?? 0
            });
        }

        // POST: Create Receipt / Order
        [HttpPost]
        public async Task<IActionResult> CreateReceipt([FromBody] ReceiptRequest request)
        {
            if (request == null || !request.Items.Any()) return Json(new { success = false, message = "Giỏ hàng rỗng." });

            // Check clock in status from DB
            var clockIn = await _context.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == request.EmployeeId && !a.ClockOutTime.HasValue);
            if (clockIn == null)
            {
                return Json(new { success = false, message = "Bạn phải Điểm danh vào ca để thực hiện bán hàng!" });
            }

            var user = await _context.Users.FindAsync(request.EmployeeId);

            var total = request.Items.Sum(i => i.Price * i.Quantity);
            decimal discount = 0;

            // Apply Membership Discount
            if (request.DiscountPercent > 0)
            {
                discount += total * (decimal)request.DiscountPercent / 100;
            }

            // Apply Voucher
            if (request.VoucherDiscountPercent > 0)
            {
                discount += (total - discount) * (decimal)request.VoucherDiscountPercent / 100;
            }
            else if (request.VoucherDiscountAmount > 0)
            {
                discount += request.VoucherDiscountAmount;
            }

            var finalTotal = total - discount;

            var receipt = new SalesReceiptModel
            {
                Id = (int)(DateTime.Now.Ticks % 1000000), // Temporary tracking ID for UI
                MaNhanVien = request.EmployeeId,
                TenNhanVien = user?.FullName ?? "Nhan vien",
                TenKhachHang = string.IsNullOrEmpty(request.CustomerName) ? "Khách lẻ" : request.CustomerName,
                NgayLap = DateTime.Now,
                TongTien = finalTotal,
                TrangThai = "Hoàn Thành",
                GhiChu = $"Bán tại quầy. Gốc: {total:N0}đ, Giảm: {discount:N0}đ. Ca: {clockIn.ClockInTime:HH:mm}"
            };

            // The following InMemory storage is now obsolete as we use _orderService for real persistence
            /*
            InMemoryDataStore.SalesReceipts.Add(receipt);

            foreach(var it in request.Items)
            {
                InMemoryDataStore.SalesReceiptDetails.Add(new SalesReceiptDetailModel
                {
                    Id = InMemoryDataStore.NextSalesReceiptDetailId,
                    MaPhieu = receipt.Id,
                    TenSanPham = it.Name,
                    SoLuong = it.Quantity,
                    DonGia = it.Price,
                    ThanhTien = it.Price * it.Quantity
                });
            }
            */

            // Create real EF Core Order so Admin can see it
            var email = HttpContext.Session.GetString("UserEmail");
            var currentUser = await _userRepository.GetByEmailAsync(email ?? "");
            int userId = currentUser?.Id ?? 1; // Fallback to 1 if no user found

            if (!string.IsNullOrEmpty(request.CustomerPhone))
            {
                var customer = await _userRepository.GetByPhoneAsync(request.CustomerPhone);
                if (customer != null) userId = customer.Id;
            }

            var cartItems = request.Items.Select(i => new FastFood.Application.DTOs.CartItemDto 
            {
                ProductId = i.IsCombo ? 0 : i.Id,
                ComboId = i.IsCombo ? i.Id : (int?)null,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            try {
                var orderDto = await _orderService.CreateOrderAsync(userId, $"Bán tại quầy: {request.CustomerName}", cartItems, discount);
                await _orderService.UpdateOrderStatusAsync(orderDto.Id, "Completed");
            } catch { }

            return Json(new { success = true, message = "Thanh toán thành công!", receiptId = receipt.Id, finalTotal = finalTotal, discount = discount });
        }

        // POST: Mark Online Order Completed
        [HttpPost]
        public async Task<IActionResult> MarkOrderCompleted(int orderId)
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, "Completed");
            return Json(new { success = success, message = success ? "Đã xử lý xong đơn hàng!" : "Không thể cập nhật trạng thái đơn hàng." });
        }
    }

    public class ReceiptRequest
    {
        public int EmployeeId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string VoucherCode { get; set; } = string.Empty;
        public int DiscountPercent { get; set; }
        public decimal VoucherDiscountAmount { get; set; }
        public int VoucherDiscountPercent { get; set; }
        public List<ReceiptItemRequest> Items { get; set; } = new();
    }

    public class ReceiptItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsCombo { get; set; } = false;
    }
}
