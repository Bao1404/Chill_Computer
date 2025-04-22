using Chill_Computer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chill_Computer.ViewModels;
using Chill_Computer.Services;
using Chill_Computer.Contacts;

namespace Chill_Computer.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ChillComputerContext _context;
        private readonly IReviewService _reviewService;

        public ProfileController(ChillComputerContext context, IReviewService reviewService)
        {
            _reviewService = reviewService;
            _context = context;
        }

        public async Task<IActionResult> Profile()
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
