using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductListPage()
        {
            return View();
        }

        public IActionResult ProductDetailPage()
        {
            return View();
        }
    }
}
