using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserRepository _userRepository;
        public RegisterController(IAccountService accountService, IUserRepository userRepository)
        {
            _accountService = accountService;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("/Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            var account = _accountService.GetAccountByUserName(model.UserName);
            if (account == null)
            {
                var newAccount = new Account()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    RoleId = 2
                };
                var newUser = new User()
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email
                };
                _accountService.AddAccount(newAccount);
                _userRepository.AddUser(newUser);
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại";
                return View("Index");
            }
        }
    }
}
