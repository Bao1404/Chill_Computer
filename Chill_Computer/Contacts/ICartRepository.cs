using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface ICartRepository
    {
        public ShoppingCart GetCartByUserId(int userId);
        public void CreateCart(ShoppingCart cart);
    }
}
