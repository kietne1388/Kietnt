using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Quản Lý Chấm Công";
            var logs = await _context.Attendances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.ClockInTime)
                .ToListAsync();
            return View(logs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _context.Attendances.FindAsync(id);
            if (log != null)
            {
                _context.Attendances.Remove(log);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa bản ghi chấm công!";
            }
            return RedirectToAction("Index");
        }
    }
}
