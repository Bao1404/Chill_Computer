using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IUserRepository
    {
        public User GetUserByUserName(string username);
    }
}
