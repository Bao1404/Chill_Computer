using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ChillComputerContext _context;
        public UserRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public User GetUserByUserName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }
    }
}
