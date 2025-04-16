using Chill_Computer.Models;

namespace Chill_Computer.ViewModels
{
    public class ManageOrderViewModel
    {
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public DateOnly OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
