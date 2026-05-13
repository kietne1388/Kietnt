using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class SalesReceiptController : Controller
    {
        private readonly AppDbContext _context;

        public SalesReceiptController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Quản Lý Phiếu Bán Hàng";
            var orders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var receipts = orders.Select(o => new SalesReceiptModel
            {
                Id = o.Id,
                MaNhanVien = o.UserId,
                TenNhanVien = o.User?.FullName ?? "Unknown",
                TenKhachHang = o.Address.StartsWith("Bán tại quầy:") ? o.Address.Replace("Bán tại quầy: ", "") : (o.User?.FullName ?? "Online"),
                NgayLap = o.CreatedAt,
                TongTien = o.TotalAmount,
                TrangThai = o.Status == "Completed" ? "Hoàn Thành" : (o.Status == "Pending" ? "Chờ Xử Lý" : "Đang Xử Lý"),
                GhiChu = o.OrderCode
            }).ToList();

            return View(receipts);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var o = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (o == null) return NotFound();

            var receipt = new SalesReceiptModel
            {
                Id = o.Id,
                MaNhanVien = o.UserId,
                TenNhanVien = o.User?.FullName ?? "Unknown",
                TenKhachHang = o.Address.StartsWith("Bán tại quầy:") ? o.Address.Replace("Bán tại quầy: ", "") : (o.User?.FullName ?? "Online"),
                NgayLap = o.CreatedAt,
                TongTien = o.TotalAmount,
                TrangThai = o.Status == "Completed" ? "Hoàn Thành" : "Đang Xử Lý",
                GhiChu = o.OrderCode
            };

            var details = o.OrderItems.Select(oi => new SalesReceiptDetailModel
            {
                Id = oi.Id,
                MaPhieu = o.Id,
                TenSanPham = oi.Product?.Name ?? "Sản phẩm",
                SoLuong = oi.Quantity,
                DonGia = oi.UnitPrice,
                ThanhTien = oi.UnitPrice * oi.Quantity
            }).ToList();

            ViewBag.Details = details;
            ViewData["Title"] = $"Phiếu Bán #{id}";
            return View(receipt);
        }
    }
}
