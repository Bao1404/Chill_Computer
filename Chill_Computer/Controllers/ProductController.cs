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
        public IActionResult ProductListPage(int id) //ProductTpyeID
        {
            Init();
            var product = _productRepository.GetProductById(id);
            var productFilter = _productTypeFilterRepository.GetByProductTypeId(id);
            List<FilterCategory> listCategory = new List<FilterCategory>();
            ViewBag.ProductList = _productRepository.GetProductByTypeId(id);
            ViewBag.ProductTypeName = _productTypeRepository.GetProductTypeById(id);
            ViewBag.ProductFilters = productFilter;
            foreach (var category in productFilter)
            {
                listCategory.AddRange(_filterCategoryRepository.GetGetCategoriesByFilterId(category.FilterId));               
            }
            ViewBag.FilterCategory = listCategory;
            ViewBag.ProductBrand = _brandRepository.GetAllBrands();
            return View(id);
        }

        public IActionResult ProductDetailPage(int id)
        {
            Init();
            var product = _productRepository.GetProductById(id);
            if (product != null)
            {
                ViewBag.ProductTypeName = _productTypeRepository.GetProductTypeById(product.TypeId.Value).TypeName;
            }
            ViewBag.ProductVersion = _productRepository.GetProductVersionFromProductName(_productRepository.GetProductById(id).ProductName);
            ViewBag.Brand = _brandRepository.GetBrandNameById(_productRepository.GetProductById(id).BrandId.Value);
            ViewBag.AttrbuteList = _attributeRepository.GetAttributeByTypeId(id);
            ViewBag.ProductAttributeList = _productAttributeRepository.GetProductAttributeByProductId(id);
            return View(_productRepository.GetProductById(id));
        }

        [HttpGet]
        public IActionResult ProductFilter(List<string> selectedFilters, List<string> selectedCategory, int productTypeId)
        {
            Init();
            List<Product> filteredProducts;

            // If filters are applied, fetch filtered list
            if (selectedFilters != null && selectedCategory != null)
            {
                filteredProducts = new List<Product>();
                foreach (var filter in selectedFilters)
                {
                    foreach(var category in selectedCategory)
                    {
                        filteredProducts.AddRange(_productRepository.GetProductFromFilter(filter, category));
                    }
                }
            }
            else
            {
                filteredProducts = _productRepository.GetProductByTypeId(productTypeId).ToList();
            }


            if (filteredProducts.Count > 0)
            {
                ViewBag.ProductList = filteredProducts;
                ViewBag.ProductTypeName = _productTypeRepository.GetProductTypeById(productTypeId);
                var productFilter = _productTypeFilterRepository.GetByProductTypeId(productTypeId);
                ViewBag.ProductFilters = productFilter;

                List<FilterCategory> listCategory = new List<FilterCategory>();
                foreach (var category in productFilter)
                {
                    listCategory.AddRange(_filterCategoryRepository.GetGetCategoriesByFilterId(category.FilterId));
                }

                ViewBag.FilterCategory = listCategory;
                ViewBag.ProductBrand = _brandRepository.GetAllBrands();

                return View("ProductListPage", productTypeId);
            }
            else
            {
                // This case should rarely happen now, but kept for fallback
                ViewBag.ProductList = _productRepository.GetProductByTypeId(productTypeId);
                ViewBag.ProductTypeName = new ProductType { TypeName = "Không có sản phẩm phù hợp" };
                ViewBag.ProductFilters = new List<ProductTypeFilter>();
                ViewBag.FilterCategory = new List<FilterCategory>();
                ViewBag.ProductBrand = _brandRepository.GetAllBrands();

                return View("ProductListPage", productTypeId);
            }
        }
    }
}
