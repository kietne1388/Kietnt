using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class IngredientController : Controller
    {
        private static List<IngredientModel> _ingredients = new List<IngredientModel>
        {
            new IngredientModel { Id = 1, TenNguyenLieu = "Cà phê nguyên hạt", DonVi = "kg", SoLuongTon = 25.5m, GiaNhap = 180000, MoTa = "Cà phê Arabica rang mộc", TrangThai = true },
            new IngredientModel { Id = 2, TenNguyenLieu = "Sữa tươi", DonVi = "lít", SoLuongTon = 40m, GiaNhap = 28000, MoTa = "Sữa tươi không đường", TrangThai = true },
            new IngredientModel { Id = 3, TenNguyenLieu = "Đường", DonVi = "kg", SoLuongTon = 30m, GiaNhap = 20000, MoTa = "Đường cát trắng", TrangThai = true },
            new IngredientModel { Id = 4, TenNguyenLieu = "Trà đen", DonVi = "kg", SoLuongTon = 10m, GiaNhap = 120000, MoTa = "Trà Ceylon", TrangThai = true },
            new IngredientModel { Id = 5, TenNguyenLieu = "Trân châu đen", DonVi = "kg", SoLuongTon = 15m, GiaNhap = 45000, MoTa = "Trân châu tapioca", TrangThai = true },
        };
        private static int _nextId = 6;

        public IActionResult Index()
        {
            ViewData["Title"] = "Quản Lý Nguyên Liệu";
            return View(_ingredients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm Nguyên Liệu";
            return View(new IngredientModel());
        }

        [HttpPost]
        public IActionResult Create(IngredientModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _nextId++;
                model.TrangThai = true;
                _ingredients.Add(model);
                TempData["Success"] = "Thêm nguyên liệu thành công!";
                return RedirectToAction("Index");
            }
            ViewData["Title"] = "Thêm Nguyên Liệu";
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _ingredients.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            ViewData["Title"] = "Sửa Nguyên Liệu";
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(IngredientModel model)
        {
            var item = _ingredients.FirstOrDefault(i => i.Id == model.Id);
            if (item == null) return NotFound();

            item.TenNguyenLieu = model.TenNguyenLieu;
            item.DonVi = model.DonVi;
            item.SoLuongTon = model.SoLuongTon;
            item.GiaNhap = model.GiaNhap;
            item.MoTa = model.MoTa;
            item.TrangThai = model.TrangThai;

            TempData["Success"] = "Cập nhật nguyên liệu thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _ingredients.FirstOrDefault(i => i.Id == id);
            if (item != null) _ingredients.Remove(item);
            TempData["Success"] = "Đã xóa nguyên liệu!";
            return RedirectToAction("Index");
        }
    }

    public class IngredientModel
    {
        public int Id { get; set; }
        public string TenNguyenLieu { get; set; } = string.Empty;
        public string DonVi { get; set; } = string.Empty;
        public decimal SoLuongTon { get; set; }
        public decimal GiaNhap { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public bool TrangThai { get; set; } = true;
    }
}
