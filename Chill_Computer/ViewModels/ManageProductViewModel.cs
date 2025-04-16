namespace Chill_Computer.ViewModels
{
    public class ManageProductViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductVersion { get; set; }
        public string? ProductType { get; set; }
        public string? ProductBrand { get; set; }
        public int TypeId { get; set; }
        public int BrandId { get; set; }
        public int Price { get; set; }
        public string? FormattedPrice { get; set; }
        public int Stock { get; set; }
        public int? SeriesId { get; set; }
    }
}
