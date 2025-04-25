using Chill_Computer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chill_Computer.ViewModels;
using Chill_Computer.Services;
using Chill_Computer.Contacts;
using System.Security.Claims;

namespace Chill_Computer.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ChillComputerContext _context;
        private readonly IReviewService _reviewService;
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _accountService;
        private readonly IOrderHistoryService _orderHistoryService;

        public ProfileController(ChillComputerContext context, IReviewService reviewService, IUserRepository userRepository, IAccountService accountService, IOrderHistoryService orderHistoryService)
        {
            _context = context;
            _reviewService = reviewService;
            _userRepository = userRepository;
            _accountService = accountService;
            _orderHistoryService = orderHistoryService;
        }


        public async Task<IActionResult> Profile()
        {
            // Lấy claim Email và UserName
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            var usernameClaim = User.FindFirst(ClaimTypes.Name);

            if (emailClaim == null && usernameClaim == null)
            {
                return Unauthorized(); // Không có thông tin xác thực
            }

            User? user = null;

            if (emailClaim != null)
            {
                string email = emailClaim.Value;
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }

            // Nếu không tìm thấy theo email và có username thì tìm theo username
            if (user == null && usernameClaim != null)
            {
                string username = usernameClaim.Value;
                user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            }

            if (user == null)
            {
                return NotFound(); // Không tìm thấy user
            }

            var userViewModel = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Dob = user.Dob
            };

            return View(userViewModel);
        }



        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == 2);
            var userViewModel = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Dob = user.Dob
            };

            return View(userViewModel);
        }

        public async Task<IActionResult> OrderHistory()
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == 2) // Chỉ lấy đơn hàng của UserId = 2  
                .Select(o => new OrderHistoryViewModel
                {
                    OrderId = o.OrderId.ToString(),
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus
                })
                .ToListAsync();

            return View(orders);
        }

        public IActionResult Review()
        {
            return View();
        }

        //public IActionResult Review(int productId)
        //{
        //    var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new ReviewViewModel
        //    {
        //        ProductId = product.ProductId,
        //        ProductName = product.ProductName
        //    };

        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> SaveProfile(ProfileViewModel model)
        {
            // Giả sử bạn đang có thông tin UserId hoặc lấy được thông qua xác thực đăng nhập  
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == 2); // hoặc lấy theo username  

            if (user == null)
            {
                return NotFound(); // hoặc xử lý phù hợp  
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            // Chỉ gán ngày sinh nếu người dùng có nhập  
            if (model.Dob.HasValue)
            {
                user.Dob = model.Dob;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("UpdateProfile");
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Review", model); // Trả lại view cùng dữ liệu lỗi  
            }

            await _reviewService.SubmitReviewAsync(model);

            // Bạn có thể trả về view thành công hoặc redirect:  
            return RedirectToAction("ReviewSuccess");
        }

        public IActionResult ReviewSuccess()
        {
            return View(); // Tạo view báo thành công nếu cần
        }

    }
}
