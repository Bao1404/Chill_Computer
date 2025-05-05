using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("_userId");
            return RedirectToAction("Index", "Home");
        }
    }
}
