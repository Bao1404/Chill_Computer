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

        public CartController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository)
        {
            _productTypeRepository = productTypeRepository;
            _context = context;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _productRepository = productRepository;
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

            if (product != null)
            {
                var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
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
                        Version = product.Version,
                        ImageUrl = product.Img1,
                        Price = product.Price,
                        Quantity = 1
                    });
                }

                HttpContext.Session.SetObject("Cart", cart);
            }

            return Json(new { success = true, count = cart.Sum(x => x.Quantity) });
        }

        public ActionResult RenderMiniCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            return PartialView("_CartPartial", cart);
        }
    }
}
