using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Chill_Computer.Helpers.Validations
{
    public class EmailValidation : ValidationAttribute
    {
        private const string emailRegex = "^[a-zA-Z0-9._]+@[a-zA-Z0-9.]+\\.[a-zA-Z]{2,6}$";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? "Email không được để trống!");
            }
            if(!Regex.IsMatch(value.ToString(), emailRegex)){
                return new ValidationResult(ErrorMessage ?? "Email không hợp lệ!");
            }
            return ValidationResult.Success;
        }
    }
}
