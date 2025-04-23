using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Chill_Computer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Chill_Computer.Controllers
{
    public class BaseController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ChillComputerContext _context;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        public BaseController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        {
            _context = context;
            _productTypeRepository = productTypeRepository;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }
        public IActionResult Init()
        {
            List<ProductTypeFilter> filters = new List<ProductTypeFilter>();
            List<FilterCategory> categories = new List<FilterCategory>();
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
            var user = HttpContext.Session.GetObject<int>("_userId");
            foreach (var typeFilter in _productTypeRepository.GetProductTypes())
            {
                filters.AddRange(_productTypeFilterRepository.GetByProductTypeId(typeFilter.TypeId));
                foreach(var category in _productTypeFilterRepository.GetByProductTypeId(typeFilter.TypeId))
                {
                    categories.AddRange(_filterCategoryRepository.GetGetCategoriesByFilterId(category.FilterId));
                }
            }
            if (user != 0)
            {
                var cartId = _cartRepository.GetCartByUserId(user);
                if (cartId != null)
                {
                    cartItems = _cartItemRepository.GetCartItemByCartId(cartId.CartId).ToList();
                }
                ViewBag.CartItems = cartItems;
            }
            ViewBag.MenuTitle = _productTypeRepository.GetProductTypes();
            ViewBag.FilterTitle = filters;
            ViewBag.Categories = categories;
            return View();
        }
    }
}
