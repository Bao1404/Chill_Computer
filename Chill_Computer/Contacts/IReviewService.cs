using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IReviewService
    {
        Task SubmitReviewAsync(ReviewViewModel model);
    }
}
