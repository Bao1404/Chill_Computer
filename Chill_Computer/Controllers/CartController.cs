using Microsoft.AspNetCore.Mvc;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Chill_Computer.Contacts;

namespace Chill_Computer.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ChillComputerContext _context;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        private readonly IProductRepository _productRepository;
        public CartController( ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository)
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
    }
}
