using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chill_Computer.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ChillComputerContext _context;

        public OrderRepository(ChillComputerContext context)
        {
            _context = context;
        }

        public List<ManageOrderViewModel> GetOrders(int pageNumber, int pageSize)
        {
            // Get the orders with user details
            var orders = (from order in _context.Orders
                          join user in _context.Users on order.UserId equals user.UserId
                          select new
                          {
                              order.OrderId,
                              CustomerName = user.FullName,
                              CustomerEmail = user.Email,
                              CustomerPhone = user.Phone,
                              CustomerAddress = user.Address,
                              OrderDate = order.OrderDate,
                              TotalAmount = order.TotalPrice,
                              FormattedTotalAmount = string.Format("{0:N0} đ", order.TotalPrice),
                              OrderStatus = order.OrderStatus
                          })
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();

            // Get the order items separately
            var orderIds = orders.Select(o => o.OrderId).ToList();
            var orderItems = _context.OrderItems
                .Where(oi => orderIds.Contains((int)oi.OrderId))
                .ToList();

            // Create the view model
            var orderViewModels = orders.Select(order => new ManageOrderViewModel
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                CustomerAddress = order.CustomerAddress,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                FormattedTotalAmount = order.FormattedTotalAmount,
                OrderStatus = order.OrderStatus,
                OrderItems = orderItems.Where(oi => oi.OrderId == order.OrderId).ToList()
            }).ToList();

            return orderViewModels;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.OrderStatus = status;
                _context.SaveChanges();
            }
        }

        public List<Product> GetProductByOrderID(int orderId)
        {
            // Fetch the products associated with the given order ID
            var products = (from orderItem in _context.OrderItems
                            join product in _context.Products on orderItem.ProductId equals product.ProductId
                            where orderItem.OrderId == orderId
                            select product).ToList();

            return products;
        }
        public Dictionary<int, List<Product>> GetOrderProductsMap()
        {
            // Lấy tất cả các OrderId hợp lệ
            var orderIds = _context.Orders.Select(o => o.OrderId).ToList();

            // Lấy tất cả các OrderItem có OrderId khác null và thuộc danh sách orderIds
            var orderItemsWithProducts = _context.OrderItems
                .Where(oi => oi.OrderId != null && orderIds.Contains(oi.OrderId.Value))
                .Join(_context.Products,
                      oi => oi.ProductId,
                      p => p.ProductId,
                      (oi, p) => new { OrderId = oi.OrderId.Value, Product = p })
                .ToList();

            // Gom nhóm theo OrderId
            var orderProductsMap = orderItemsWithProducts
                .GroupBy(x => x.OrderId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.Product).ToList()
                );

            return orderProductsMap;
        }

        public List<ManageOrderViewModel> GetOrderBySearchCustomerName(string customerName)
        {

            var orders = (from order in _context.Orders
                          join user in _context.Users on order.UserId equals user.UserId
                          where user.FullName.Contains(customerName)
                          select new
                          {
                              order.OrderId,
                              CustomerName = user.FullName,
                              CustomerEmail = user.Email,
                              CustomerPhone = user.Phone,
                              CustomerAddress = user.Address,
                              OrderDate = order.OrderDate,
                              TotalAmount = order.TotalPrice,
                              FormattedTotalAmount = string.Format("{0:N0} đ", order.TotalPrice),
                              OrderStatus = order.OrderStatus
                          }).ToList();

            var orderViewModels = orders.Select(order => new ManageOrderViewModel
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                CustomerAddress = order.CustomerAddress,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                FormattedTotalAmount = order.FormattedTotalAmount,
                OrderStatus = order.OrderStatus
            }).ToList();
            return orderViewModels;
        }

        public List<ManageOrderViewModel> GetOrderBySearchCustomerName(string customerName, int pageNumber, int pageSize)
        {

            var orders = (from order in _context.Orders
                          join user in _context.Users on order.UserId equals user.UserId
                          where user.FullName.Contains(customerName)
                          select new
                          {
                              order.OrderId,
                              CustomerName = user.FullName,
                              CustomerEmail = user.Email,
                              CustomerPhone = user.Phone,
                              CustomerAddress = user.Address,
                              OrderDate = order.OrderDate,
                              TotalAmount = order.TotalPrice,
                              FormattedTotalAmount = string.Format("{0:N0} đ", order.TotalPrice),
                              OrderStatus = order.OrderStatus
                          }).ToList();

            var orderViewModels = orders.Select(order => new ManageOrderViewModel
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                CustomerAddress = order.CustomerAddress,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                FormattedTotalAmount = order.FormattedTotalAmount,
                OrderStatus = order.OrderStatus
            }).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return orderViewModels;
        }


    }
}
