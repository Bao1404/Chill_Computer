using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
