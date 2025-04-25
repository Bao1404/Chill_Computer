using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IOrderHistoryService
    {
        public List<OrderHistoryViewModel> GetOrderHistories(int pageNumber, int pageSize);
    }
}
