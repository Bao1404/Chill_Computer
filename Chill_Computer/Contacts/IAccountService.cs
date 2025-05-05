using Chill_Computer.Models;
using Chill_Computer.ViewModels;


namespace Chill_Computer.Contacts

{
    public interface IAccountService
    {
        List<AccountViewModel> GetAccounts(int pageNumber, int pageSize);
        public Account GetAccountByUserId(int userId);
        public List<AccountViewModel> SearchByUsername(string username, int pageNumber, int pageSize);
        public List<AccountViewModel> SearchByUsername(string username);
        public AccountViewModel GetAccountVMByUserName(string username);
        public Account GetAccountByUserName(string username);
        public void UpdateRole(Account a, int roleId);
        public void DeleteAccount(Account a);
        public Account GetAccountByNameAndPass(string username, string password);
        public bool IsExistAccount(string username);
        public void AddAccount(Account account);
    }
}
