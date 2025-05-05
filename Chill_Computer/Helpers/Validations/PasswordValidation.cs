using System.ComponentModel.DataAnnotations;

namespace Chill_Computer.Helpers.Validations
{
    public class PasswordValidation : ValidationAttribute
    {
        private const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
    }
}
