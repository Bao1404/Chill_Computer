using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ChillComputerContext _context;

        public ReviewService(ChillComputerContext context)
        {
            _context = context;
        }


        public async Task<bool> HasUserReviewedProductAsync(int userId, int productId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task SubmitReviewAsync(ReviewViewModel model)
        {
            var review = new Review
            {
                ProductId = model.ProductId,
                Rating = model.Rating,
                Comment = model.Comment,
                CreatedAt = DateTime.Now,
                UserId = model.UserId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReviewViewModel>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => new ReviewViewModel
                {
                    ProductId = r.ProductId,
                    Rating = r.Rating ?? 0, 
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UserId = r.UserId
                }).ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => (double)(r.Rating ?? 0));
        }

        public async Task DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);

            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}

