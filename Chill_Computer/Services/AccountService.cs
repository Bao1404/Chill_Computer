using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Services
{
    public class AccountService : IAccountService
    {
        private readonly ChillComputerContext _context;

        public AccountService(ChillComputerContext context)
        {
            _context = context;
        }

        public List<AccountViewModel> GetAccounts(int pageNumber, int pageSize)
        {
            return (from account in _context.Accounts
                    join user in _context.Users on account.UserName equals user.UserName
                    join role in _context.Roles on account.RoleId equals role.RoleId
                    select new AccountViewModel
                    {
                        Id = user.UserId,
                        Username = user.UserName,
                        FullName = user.FullName,
                        Role = role.RolePosition,
                        RoleId = role.RoleId
                    })
                    .Skip((pageNumber - 1) * pageSize) 
                    .Take(pageSize) 
                    .ToList();
        }

        public AccountViewModel GetAccountVMByUserName(string username)
        {
            if (IsExistAccount(username))
            {
                return (
                    from account in _context.Accounts
                    join user in _context.Users on account.UserName equals user.UserName
                    join role in _context.Roles on account.RoleId equals role.RoleId
                    where account.UserName == username
                    select new AccountViewModel
                    {
                        Id = user.UserId,
                        Username = user.UserName,
                        FullName = user.FullName,
                        Role = role.RolePosition,
                        RoleId = role.RoleId
                    }).FirstOrDefault();
            }

            return null;
        }

        public void UpdateRole(Account a, int roleId)
        {
            a.RoleId = roleId;
            _context.Accounts.Update(a);
            _context.SaveChanges();
        }

        public void DeleteAccount(Account account)
        {
            // Step 1: Load the associated User for the account
            var user = _context.Users
                               .Include(u => u.ShoppingCarts) // Include related ShoppingCart
                               .Include(u => u.Orders) // Include related Orders
                               .Include(u => u.Payments) // Include related Payments
                               .FirstOrDefault(u => u.UserName == account.UserName);

            if (user != null)
            {
                // Step 2: Delete related ShoppingCart items
                var shoppingCarts = _context.ShoppingCarts.Where(sc => sc.UserId == user.UserId).ToList();
                foreach (var cart in shoppingCarts)
                {
                    // Remove all items in the ShoppingCart
                    var cartItems = _context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();
                    _context.CartItems.RemoveRange(cartItems);
                    _context.ShoppingCarts.Remove(cart);
                }

                // Step 3: Delete related Orders
                var orders = _context.Orders.Where(o => o.UserId == user.UserId).ToList();
                foreach (var order in orders)
                {
                    // Remove all OrderItems related to the order
                    var orderItems = _context.OrderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                    _context.OrderItems.RemoveRange(orderItems);
                    _context.Orders.Remove(order);
                }

                // Step 4: Delete related Payments
                var payments = _context.Payments.Where(p => p.UserId == user.UserId).ToList();
                _context.Payments.RemoveRange(payments);

                // Step 5: Remove the user record
                _context.Users.Remove(user);
            }

            // Step 6: Finally, remove the account
            _context.Accounts.Remove(account);

            // Save all changes to the database
            _context.SaveChanges();
        }

        public Account GetAccountByUserName(string username)
        {
            return _context.Accounts.FirstOrDefault(x => x.UserName == username);
        }
        public bool IsExistAccount(string username)
        {
            if (_context.Accounts.Any(x => x.UserName == username))
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
