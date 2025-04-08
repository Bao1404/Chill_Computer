using Microsoft.AspNetCore.Mvc;
using Chill_Computer.Models;
using Chill_Computer.Services;

namespace Chill_Computer.Controllers
{
    public class CartController : BaseController
    {
        private readonly ChillComputerContext _context;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        public CartController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository)
            : base(context, productTypeRepository, productTypeFilterRepository)
        {
            _context = context;
            _productTypeRepository = productTypeRepository;
            _productTypeFilterRepository = productTypeFilterRepository;
        }
    
        public IActionResult CartPage()
        {
            Init();
            return View();
        }
    }
}
