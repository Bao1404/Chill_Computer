namespace Chill_Computer.ViewModels
{
    public class CartItemViewModel
    {
        public int? ProductId { get; set; }
        public int? PcId { get; set; }
        public string? ProductName { get; set; }
        public string? Version { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string? FormattedPrice { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }
}
