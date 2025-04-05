using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IAccountService
    {
        List<AccountViewModel> GetAccounts(int pageNumber, int pageSize);
        public AccountViewModel GetAccountVMByUserName(string username);
        public Account GetAccountByUserName(string username);
        public void UpdateRole(Account a, int roleId);
        public void DeleteAccount(Account a);
        public bool IsExistAccount(string username);
    }
}
