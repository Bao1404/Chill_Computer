using System.ComponentModel.DataAnnotations;

namespace Chill_Computer.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
