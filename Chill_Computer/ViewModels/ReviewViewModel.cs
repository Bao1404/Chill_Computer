using Chill_Computer.Models;

namespace Chill_Computer.ViewModels
{
    public class ReviewViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        //public string ImageUrl { get; set; } // Đường dẫn ảnh sản phẩm
    }
}
