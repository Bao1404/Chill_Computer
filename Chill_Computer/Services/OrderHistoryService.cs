using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly ChillComputerContext _context;

        public OrderHistoryService(ChillComputerContext context)
        {
            _context = context;
        }

        public List<OrderHistoryViewModel> GetOrderHistories(int pageNumber, int pageSize)
        {
            return (from order in _context.Orders
                    select new OrderHistoryViewModel
                    {
                        OrderId = order.OrderId.ToString(),
                        OrderDate = order.OrderDate,
                        TotalPrice = order.TotalPrice,
                        OrderStatus = order.OrderStatus
                    })
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }
    }
}
