using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            // Placeholder for admin settings
            return View();
        }
    }
}
