using Chill_Computer.Helpers.Validations;
using System.ComponentModel.DataAnnotations;

namespace Chill_Computer.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Không được để trống.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Không được để trống.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Không được để trống.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Không được để trống.")]
        [EmailValidation]
        public string Email { get; set; }

    }
}
