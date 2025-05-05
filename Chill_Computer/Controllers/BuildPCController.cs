using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Chill_Computer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class BuildPCController : BaseController
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ChillComputerContext _context;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IPcComponentRepository _pcComponentRepository;
        private readonly IPCRepository _pcRepository;

        public BuildPCController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository, IBrandRepository brandRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IPCRepository pCRepository, IPcComponentRepository pcComponentRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository, cartRepository, cartItemRepository)
        {
            _productTypeRepository = productTypeRepository;
            _context = context;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _pcRepository = pCRepository;
            _pcComponentRepository = pcComponentRepository;
        }
        public IActionResult BuildPage()
        {
            Init();
            ViewBag.ProductList = _productRepository.GetAllProducts();
            return View();
        }
        [HttpPost]
        public IActionResult AddProductBuildPcToCart(IFormCollection form)
        {
            var userId = HttpContext.Session.GetObject<int>("_userId");
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
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

            foreach (var key in keys)
            {
                var value = form[key];
                if (int.TryParse(value, out var id))
                {
                    selectedIds.Add(id);
                }
            }

            // Parse total price safely
            int price = 0;
            if (!int.TryParse(totalPrice, out price))
            {
                price = 0; // fallback to 0 if parse fails
            }

            var formattedPrice = String.Format("{0:N0}₫", price);

            if (userId != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(userId).CartId;
                var userCart = _cartRepository.GetCartByUserId(userId);
                if (userCart == null)
                {
                    userCart = new ShoppingCart
                    {
                        UserId = userId,
                    };
                    _cartRepository.CreateCart(userCart);
                }

                Pc pc = new Pc
                {
                    Price = price,
                    //FormattedPrice = formattedPrice
                };

                _pcRepository.AddPc(pc);
                _pcComponentRepository.AddListProductToPC(pc.PcId, selectedIds);
                _cartItemRepository.AddCartItem(new CartItem
                {
                    PcId = pc.PcId,
                    ItemQuantity = 1,
                    CartId = cartId
                });
            }
            else
            {
                // For guests, simulate the "custom PC" as a CartItemViewModel
                var guestPc = new CartItemViewModel
                {
                    PcId = 0, // You may use a temp ID or handle PcId = null for guest PCs
                    ProductName = "Custom PC Build",
                    Price = price,
                    FormattedPrice = formattedPrice,
                    Quantity = 1
                };

                cart.Add(guestPc);
                HttpContext.Session.SetObject("Cart", cart);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
