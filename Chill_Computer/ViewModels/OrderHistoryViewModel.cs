namespace Chill_Computer.ViewModels
{
    public class OrderHistoryViewModel
    {
        public string OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public List<DetailsViewModel> Items { get; set; }
    }
}
