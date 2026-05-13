using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            // Placeholder for future notification feature
            return View(new List<string>());
        }
    }
}
