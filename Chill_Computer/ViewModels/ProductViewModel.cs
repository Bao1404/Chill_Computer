using Chill_Computer.Models;

namespace Chill_Computer.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Price { get; set; }

        public string? FormattedPrice { get; set; }

        public int Stock { get; set; }

        public string? Img1 { get; set; }

        public string? Img2 { get; set; }

        public string? Img3 { get; set; }

        public string? Color { get; set; }

        public string? Version { get; set; }

        public int? TypeId { get; set; }

        public int? BrandId { get; set; }

        public int? SeriesId { get; set; }
    }
}
