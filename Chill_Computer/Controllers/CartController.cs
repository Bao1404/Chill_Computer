using Microsoft.AspNetCore.Mvc;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Chill_Computer.Contacts;
using Chill_Computer.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Chill_Computer.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ChillComputerContext _context;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CartController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository, cartRepository, cartItemRepository)
        {
            _productTypeRepository = productTypeRepository;
            _context = context;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public IActionResult CartPage()
        {
            Init();
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var userId = HttpContext.Session.GetObject<int>("_userId");

            if (product != null)
            {
                var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
                if (userId != 0)
                {
                    var userCart = _cartRepository.GetCartByUserId(userId);
                    if(userCart == null)
                    {
                        userCart = new ShoppingCart
                        {
                            UserId = userId,
                        };
                        _cartRepository.CreateCart(userCart);
                    }

                    CartItem cartItem = new CartItem
                    {
                        ProductId = productId,
                        ItemQuantity = 1,
                        CartId = userCart.CartId
                    };
                    _cartItemRepository.AddCartItem(cartItem);
                }
                else
                {
                    if (existingItem != null)
                    {
                        existingItem.Quantity += 1;
                    }
                    else
                    {
                        cart.Add(new CartItemViewModel
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            FormattedPrice = product.FormattedPrice,
                            Version = product.Version,
                            ImageUrl = product.Img1,
                            Price = product.Price,
                            Quantity = 1
                        });

                    }
                }

                HttpContext.Session.SetObject("Cart", cart);
            }

            return Json(new { success = true, count = cart.Sum(x => x.Quantity) });
        }

        public ActionResult RenderMiniCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            return PartialView("Partials/_CartPartial", cart);
        }
    }
}
