namespace Chill_Computer.ViewModels
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }

        public string Email { get; set; } = null!;

        public string Phone { get; set; }

        public DateOnly? Dob { get; set; }

        public string UserName { get; set; } = null!;
    }
}
