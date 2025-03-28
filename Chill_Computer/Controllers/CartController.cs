using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult CartPage()
        {
            return View();
        }
    }
}
