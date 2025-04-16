using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IOrderRepository
    {
        public List<ManageOrderViewModel> GetOrders(int pageSize, int pageNumber);
        public void AddOrder(Order order);
        public void UpdateOrder(Order order);
        public void DeleteOrder(int orderId);
        public Order GetOrderById(int orderId);
        public void UpdateOrderStatus(int orderId, string status);
    }
}
