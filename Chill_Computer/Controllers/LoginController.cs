using Chill_Computer.Contacts;
using Chill_Computer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserRepository _userRepository;
        public LoginController(IAccountService accountService, IUserRepository userRepository)
        {
            _accountService = accountService;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }
        [HttpPost("/Login")]
        public IActionResult Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var account = _accountService.GetAccountByNameAndPass(model.Username, model.Password);
            if(account != null)
            {
                var user = _userRepository.GetUserByUserName(account.UserName);
                HttpContext.Session.SetObject("_userId", user.UserId);
                HttpContext.Session.SetObject("_userRole", account.Role.RolePosition);
                HttpContext.Session.SetString("_userFullName", user.FullName);
                if (account.RoleId == 1)
                {
                    return RedirectToAction("Admin", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
                return View("Index");
            }
        }
    }
}
