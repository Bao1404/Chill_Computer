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
                       join product in _context.Products on cartItem.ProductId equals product.ProductId
                       join pc in _context.Pcs on cartItem.PcId equals pc.PcId
                       where cartItem.CartId == cartId
                       select new
                       {
                           product.ProductId,
                           product.ProductName,
                           cartItem.ItemQuantity,
                           product.Img1,
                           ProductPrice = product.Price,
                           product.Version,
                           product.Color,
                           pc.PcId,
                           PcPrice = pc.Price
                       };

            var groupedItems = list
                .GroupBy(i => new { i.ProductId, i.ProductName, i.Img1, i.ProductPrice, i.Version, i.Color, i.PcId })
                .Select(g => new CartItemViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(i => i.ItemQuantity),
                    ImageUrl = g.Key.Img1,
                    Price = g.Key.ProductPrice,
                    Version = g.Key.Version,
                    Color = g.Key.Color,
                    FormattedPrice = g.Key.ProductPrice.ToString("N0"),
                    PcId = g.Key.PcId,
                }).ToList();

            return groupedItems;
        }
        public void DeleteItemByProductIdAndCartId(int productId, int cartId)
        {
            var item = _context.CartItems.FirstOrDefault(i => i.ProductId == productId && i.CartId == cartId);
            if(item != null)
            {
                _context.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}
