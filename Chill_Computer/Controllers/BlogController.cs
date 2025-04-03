using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult BlogPage()
        {
            return View();
        }
    }
}
