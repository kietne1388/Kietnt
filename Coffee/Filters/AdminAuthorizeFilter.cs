using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FastFood.Filters
{
    public class AdminAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (!userId.HasValue || userRole != "Admin")
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "Guest" });
            }
        }
    }
}
