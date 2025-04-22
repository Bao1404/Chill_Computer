using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Services
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ChillComputerContext _context;
        public CartItemRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public void AddCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
        }
        public List<CartItemViewModel> GetCartItemByCartId(int cartId)
        {
            var list = from cartItem in _context.CartItems
                       where cartItem.CartId == cartId
                       select new CartItemViewModel
                       {
                           ProductId = cartItem.ProductId,
                           Quantity = cartItem.ItemQuantity
                       };
            return list.ToList();
        }
    }
}
