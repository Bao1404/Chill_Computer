using Chill_Computer.Models;
using Chill_Computer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class BuildPCController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ChillComputerContext _context;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;

        public BuildPCController(ILogger<HomeController> logger, ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository)
            : base(context, productTypeRepository, productTypeFilterRepository)
        {
            _logger = logger;
            _productTypeRepository = productTypeRepository;
            _context = context;
            _productTypeFilterRepository = productTypeFilterRepository;
        }
        public IActionResult BuildPage()
        {
            Init();
            return View();
        }
    }
}
