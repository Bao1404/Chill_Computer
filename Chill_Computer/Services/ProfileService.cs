using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ChillComputerContext _context;

        public ProfileService(ChillComputerContext context)
        {
            _context = context;
        }

        public List<ProfileViewModel> GetAccounts()
        {
            return (from account in _context.Accounts
                    join user in _context.Users on account.UserName equals user.UserName
                    select new ProfileViewModel
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Dob = (DateOnly)user.Dob
                    })                  
                    .ToList();
        }

        public ProfileViewModel PostProfile(string username)
        {

            if (IsExistAccount(username))
            {
                return (from account in _context.Accounts
                        join user in _context.Users on account.UserName equals user.UserName
                        where account.UserName == username
                        select new ProfileViewModel
                        {
                            FullName = user.FullName,
                            Email = user.Email,
                            Phone = user.Phone,
                            Dob = (DateOnly)user.Dob
                        }).FirstOrDefault();
            }
            return null;
        }

        public bool IsExistAccount(string username)
        {
            if (_context.Accounts.Any(x => x.UserName == username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
