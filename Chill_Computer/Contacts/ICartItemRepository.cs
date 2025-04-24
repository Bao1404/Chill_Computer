using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface ICartItemRepository
    {
        public void AddCartItem(CartItem cartItem);
        public List<CartItemViewModel> GetCartItemByCartId(int cartId);
        public void DeleteItemByProductIdAndCartId(int productId, int cartId);
    }
}
