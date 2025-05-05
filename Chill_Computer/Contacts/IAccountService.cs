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


        public List<News> GetAllNewsPending(int pageNumber, int pageSize);
        public void AcceptNewsPending(int newsId);
        public void RejectNewsPending(int newsId);
        public List<NewsCategory> GetAllNewsCategories();
        public void AddNewArticle(News news);
        public List<News> GetAllNews(int pageNumber, int pageSize);
        public void EditNews(int idNew, News updatedNews);
        public void DeleteNews(int idNew);

        public News GetNewsById(int idNew);
    }
}
