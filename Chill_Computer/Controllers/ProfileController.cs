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
            // Lấy userId từ Session
            var userId = HttpContext.Session.GetObject<int>("_userId");

            // Nếu chưa có userId trong Session thì trả về Unauthorized
            if (userId == 0)
            {
                return Unauthorized();
            }

            // Truy vấn dữ liệu người dùng từ database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound(); // Không tìm thấy user
            }

            // Tạo ViewModel để truyền qua View
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
            var userId = HttpContext.Session.GetObject<int>("_userId");

            if (userId == 0)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
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

        [HttpPost]
        public async Task<IActionResult> SaveProfile(ProfileViewModel model)
        {
            var userId = HttpContext.Session.GetObject<int>("_userId");

            if (userId == 0)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            if (model.Dob.HasValue)
            {
                user.Dob = model.Dob;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("UpdateProfile");
        }

        public async Task<IActionResult> OrderHistory()
        {
            var userId = HttpContext.Session.GetObject<int>("_userId");

            if (userId == 0)
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
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

        public IActionResult Review(int productId)
        {
            var userId = HttpContext.Session.GetObject<int>("_userId");
            if (userId == 0) return Unauthorized();

            var model = new ReviewViewModel { ProductId = productId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ReviewViewModel model)
        {
            var userId = HttpContext.Session.GetObject<int>("_userId");
            if (userId == 0)
            {
                return Unauthorized(); // Chưa đăng nhập
            }

            if (!ModelState.IsValid)
            {
                return View("Review", model); // Form lỗi, trả về form với lỗi
            }

            model.UserId = userId;

            bool alreadyReviewed = await _reviewService.HasUserReviewedProductAsync(userId, model.ProductId);
            if (alreadyReviewed)
            {
                ModelState.AddModelError("", "Bạn đã đánh giá sản phầm này.");
                return View("Review", model);
            }

            try
            {
                await _reviewService.SubmitReviewAsync(model);
                return RedirectToAction("ReviewSuccess");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while submitting your review. Please try again later.");
                return View("Review", model);
            }
        }


        public async Task<IActionResult> List(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            var avgRating = await _reviewService.GetAverageRatingAsync(productId);

            ViewBag.AvgRating = avgRating;
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var userId = HttpContext.Session.GetObject<int>("_userId");
            if (userId == 0) return Unauthorized();

            await _reviewService.DeleteReviewAsync(reviewId, userId);
            return RedirectToAction("List");
        }
    }
}
