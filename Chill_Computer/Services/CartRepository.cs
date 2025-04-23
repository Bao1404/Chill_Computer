using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly ChillComputerContext _context;
        public CartRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public ShoppingCart GetCartByUserId(int userId)
        {
            return _context.ShoppingCarts.FirstOrDefault(c => c.UserId == userId);
        }
        public void CreateCart(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
            _context.SaveChanges();
        }
    }
}
