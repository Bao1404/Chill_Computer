using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ChillComputerContext _context;

        public AdminController(IAccountService accountService, ChillComputerContext context)
        {
            _accountService = accountService;
            _context = context;
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult ManageInventory()
        {
            return View();
        }

        public IActionResult ManageOrder()
        {
            return View();
        }
        public IActionResult ManageAccount(int pageNumber = 1, int pageSize = 7)
        {
            var accounts = _accountService.GetAccounts(pageNumber, pageSize);
            var totalAccounts = _context.Accounts.Count(); // Total count for pagination
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalAccounts / pageSize); // Calculate total pages
            ViewBag.CurrentPage = pageNumber; // Current page number
            return View(accounts);
        }

        [HttpPost]
        public IActionResult UpdateRole()
        {
            var username = Request.Form["username"];
            var roleId = int.Parse(Request.Form["roleId"]);
            var account = _accountService.GetAccountByUserName(username);
            _accountService.UpdateRole(account, roleId);
            return RedirectToAction("ManageAccount");
        }

        [HttpPost]
        public IActionResult DeleteAccount()
        {
            string username = Request.Form["usernameDe"]; // Get username from the form

            // Step 1: Find the account by username
            var account = _context.Accounts.FirstOrDefault(a => a.UserName == username);

            if (account != null)
            {
                // Step 2: Call the AccountService to delete the account
                _accountService.DeleteAccount(account);
            }

            return RedirectToAction("ManageAccount");
        }
    }
}
