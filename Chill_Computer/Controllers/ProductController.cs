using Chill_Computer.Models;
using Chill_Computer.Repository;
using Chill_Computer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ChillComputerContext _context;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;

        public ProductController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository) 
            : base(context, productTypeRepository, productTypeFilterRepository)
        {
            _context = context;
            _productTypeRepository = productTypeRepository;
            _productTypeFilterRepository = productTypeFilterRepository;
        }

        public IActionResult ProductListPage()
        {
            Init();
            return View();
        }

        public IActionResult ProductDetailPage()
        {
            return View();
        }
    }
}
