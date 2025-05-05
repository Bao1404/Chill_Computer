using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Microsoft.Data.SqlClient;

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
            try
            {
                return _context.Users.FirstOrDefault(u => u.UserName == username);
            }
            catch (SqlException se)
            {
                throw new Exception(se.Message, se);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
