using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IProfileService
    {
        List<ProfileViewModel> GetAccounts();
        ProfileViewModel PostProfile(string username);
        public bool IsExistAccount(string username);
    }
}
