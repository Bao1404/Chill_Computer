using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
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

        public BuildPCController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository, IBrandRepository brandRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository)
        {
            _productTypeRepository = productTypeRepository;
            _context = context;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
        }
        public IActionResult BuildPage()
        {
            Init();
            ViewBag.ProductList = _productRepository.GetAllProducts();
            return View();
        }
    }
}
