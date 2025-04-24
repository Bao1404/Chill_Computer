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
        private readonly IPcComponentRepository _pcComponentRepository;
        private readonly IPCRepository _pcRepository;

        public CartController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IPCRepository pcRepository, IPcComponentRepository pcComponentRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository, cartRepository, cartItemRepository)
        {
            _productTypeRepository = productTypeRepository;
            _context = context;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _pcComponentRepository = pcComponentRepository;
            _pcRepository = pcRepository;
        }

        public IActionResult CartPage()
        {
            Init();
            var userId = HttpContext.Session.GetObject<int>("_userId");
            List<CartItemViewModel> listItem = new List<CartItemViewModel>();
            if(userId != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(userId).CartId;
                if(cartId != null)
                {
                    listItem = _cartItemRepository.GetCartItemByCartId(cartId);
                }
                ViewBag.CartItems = listItem;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
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

        [HttpDelete]
        public IActionResult DeleteCartItem(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var userId = HttpContext.Session.GetObject<int>("_userId");
            if (userId != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(userId).CartId;
                _cartItemRepository.DeleteItemByProductIdAndCartId(productId, cartId);
            }
            else
            {
                var removedItem = cart.FirstOrDefault(c => c.ProductId == productId);
                if(removedItem != null)
                {
                    cart.Remove(removedItem);
                    HttpContext.Session.SetObject("Cart", cart);
                }
            }
            return Json(new { success = true, count = cart.Sum(x => x.Quantity) });
        }
        public IActionResult RenderMiniCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var userId = HttpContext.Session.GetObject<int>("_userId");
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
            if (userId != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(userId);
                if (cartId != null)
                {
                    cartItems = _cartItemRepository.GetCartItemByCartId(cartId.CartId).ToList();
                }
                 ViewBag.CartItems = cartItems;
                return PartialView("Partials/_CartPartial", ViewBag.CartItems);
            }
            else
            {
                return PartialView("Partials/_CartPartial", cart);
            }
        }

        public IActionResult RenderCartButton()
        {
            Init();
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var userId = HttpContext.Session.GetObject<int>("_userId");
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
            if (userId != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(userId);
                if (cartId != null)
                {
                    cartItems = _cartItemRepository.GetCartItemByCartId(cartId.CartId).ToList();
                }
                ViewBag.CartItems = cartItems;
                return PartialView("Partials/_CartButton", ViewBag.CartItems);
            }
            else
            {
                return PartialView("Partials/_CartButton", cart);
            }
        }

        [HttpPost]
        public IActionResult AddProductBuildPcToCart(IFormCollection form)
        {   
            var userId = HttpContext.Session.GetObject<int>("_userId");
            var selectedIds = new List<int>();
            var totalPrice = form["TotalPrice"];
            var keys = new[]
            {
                "selectedCpuId",
                "selectedMainboardId",
                "selectedVgaId",
                "selectedRamId",
                "selectedSsdId",
                "selectedHddId",
                "selectedCaseId",
                "selectedPsuId",
                "selectedTanNhietId",
                "selectedMonitorId",
                "selectedMouseId",
                "selectedKeyboardId",
                "selectedEarphoneId"
            };

            if (userId != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(userId).CartId;
                foreach (var key in keys)
                {
                    var value = form[key];

                    if (int.TryParse(value, out var id))
                    {
                        selectedIds.Add(id);
                    }
                }

                Pc pc = new Pc
                {
                    Price = decimal.Parse(totalPrice),
                };

                _pcRepository.AddPc(pc);
                _pcComponentRepository.AddListProductToPC(pc.PcId, selectedIds);
                _cartItemRepository.AddCartItem(new CartItem
                {
                    ProductId = pc.PcId,
                    ItemQuantity = 1,
                    CartId = cartId
                });
            }

            return View();
        }
    }
}
