using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using FastFood.Filters;
using FastFood.Models;
using FastFood.Application.Interfaces;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Entities;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public EmployeeController(IWebHostEnvironment env, IUserRepository userRepository, IAuthService authService)
        {
            _env = env;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Quản Lý Nhân Viên";
            var users = await _userRepository.GetAllAsync();
            var employees = users
                .Where(u => u.Role == "Staff" || u.Role == "Admin")
                .Select(u => new EmployeeModel
                {
                    Id = u.Id,
                    HoTen = u.FullName,
                    ChucVu = u.Position ?? u.Role,
                    Email = u.Email,
                    SoDienThoai = u.PhoneNumber,
                    LuongCoBan = u.BaseSalary,
                    NgayVaoLam = u.JoinDate,
                    TrangThai = u.IsActive
                }).ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm Nhân Viên";
            return View(new EmployeeModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeModel model, string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new FastFood.Domain.Entities.User
                {
                    Username = username,
                    FullName = model.HoTen,
                    Email = model.Email,
                    PhoneNumber = model.SoDienThoai,
                    PasswordHash = _authService.HashPassword(password),
                    Role = "Staff",
                    Position = model.ChucVu,
                    BaseSalary = model.LuongCoBan,
                    JoinDate = model.NgayVaoLam,
                    IsActive = true
                };

                await _userRepository.AddAsync(user);
                TempData["Success"] = "Thêm nhân viên và tài khoản thành công!";
                return RedirectToAction("Index");
            }
            ViewData["Title"] = "Thêm Nhân Viên";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return NotFound();

            var emp = new EmployeeModel
            {
                Id = u.Id,
                HoTen = u.FullName,
                ChucVu = u.Position ?? u.Role,
                Email = u.Email,
                SoDienThoai = u.PhoneNumber,
                LuongCoBan = u.BaseSalary,
                NgayVaoLam = u.JoinDate,
                TrangThai = u.IsActive
            };
            ViewData["Title"] = "Sửa Nhân Viên";
            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeModel model, string? newPassword)
        {
            var userEntity = await _userRepository.GetByIdAsync(model.Id);
            if (userEntity == null) return NotFound();

            if (!string.IsNullOrEmpty(newPassword))
            {
                userEntity.PasswordHash = _authService.HashPassword(newPassword);
            }

            userEntity.FullName = model.HoTen;
            userEntity.Position = model.ChucVu;
            userEntity.Email = model.Email;
            userEntity.PhoneNumber = model.SoDienThoai;
            userEntity.BaseSalary = model.LuongCoBan;
            userEntity.JoinDate = model.NgayVaoLam;
            userEntity.IsActive = model.TrangThai;

            await _userRepository.UpdateAsync(userEntity);

            TempData["Success"] = "Cập nhật nhân viên thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                TempData["Success"] = "Đã xóa nhân viên và tài khoản!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _userRepository.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}
