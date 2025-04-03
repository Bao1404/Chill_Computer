using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class BuildPCController : Controller
    {
        public IActionResult BuildPage()
        {
            return View();
        }
    }
}
