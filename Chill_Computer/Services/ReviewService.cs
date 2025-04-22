using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ChillComputerContext _context;

        public ReviewService(ChillComputerContext context)
        {
            _context = context;
        }

        public async Task SubmitReviewAsync(ReviewViewModel model)
        {
            var review = new Review
            {
                ProductId = model.ProductId,
                Rating = model.Rating,
                Comment = model.Comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}

