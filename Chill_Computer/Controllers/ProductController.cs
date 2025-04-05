using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;

using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ChillComputerContext _context;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository, IAttributeRepository attributeRepository, IProductAttributeRepository productAttributeRepository, IBrandRepository brandRepository) 
            : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository)
        {
            _context = context;
            _productTypeRepository = productTypeRepository;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _productRepository = productRepository;
            _attributeRepository = attributeRepository;
            _productAttributeRepository = productAttributeRepository;
            _brandRepository = brandRepository;
        }

        public IActionResult ProductListPage(int id)
        {
            Init();
            ViewBag.ProductList = _productRepository.GetProductByTypeId(id);
            return View(id);
        }

        public IActionResult ProductDetailPage(int id)
        {
            Init();
            ViewBag.ProductVersion = _productRepository.GetProductVersionFromProductName(_productRepository.GetProductById(id).ProductName);
            ViewBag.Brand = _brandRepository.GetBrandNameById(_productRepository.GetProductById(id).BrandId.Value);
            ViewBag.AttrbuteList = _attributeRepository.GetAttributeByTypeId(id);
            ViewBag.ProductAttributeList = _productAttributeRepository.GetProductAttributeByProductId(id);
            return View(_productRepository.GetProductById(id));
        }
    }
}
