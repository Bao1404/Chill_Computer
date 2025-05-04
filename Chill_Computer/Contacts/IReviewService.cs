using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IReviewService
    {
        Task SubmitReviewAsync(ReviewViewModel model);
        Task<bool> HasUserReviewedProductAsync(int userId, int productId);
        Task<List<ReviewViewModel>> GetReviewsByProductIdAsync(int productId);
        Task<double> GetAverageRatingAsync(int productId);
        Task DeleteReviewAsync(int reviewId, int userId);
    }
}
