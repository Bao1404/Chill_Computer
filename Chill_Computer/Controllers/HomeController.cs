using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chill_Computer.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductTypeRepository _productTypeRepository;
    private readonly ChillComputerContext _context;
    private readonly IProductTypeFilterRepository _productTypeFilterRepository;
    private readonly IFilterCategoryRepository _filterCategoryRepository;
    private readonly IProductRepository _productRepository;

    public HomeController(ILogger<HomeController> logger, ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository)
    {
        _logger = logger;
        _productTypeRepository = productTypeRepository;
        _context = context;
        _productTypeFilterRepository = productTypeFilterRepository;
        _filterCategoryRepository = filterCategoryRepository;
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        Init();
        List<Product> productList = new List<Product>();
        foreach(var type in _productTypeRepository.GetProductTypes())
        {
            productList = _productRepository.GetProductByTypeId(type.TypeId);
        }
        ViewBag.ProductList = productList;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
