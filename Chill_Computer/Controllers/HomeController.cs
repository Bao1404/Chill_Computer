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
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public HomeController(ILogger<HomeController> logger, ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, IProductRepository productRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        : base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository, cartRepository, cartItemRepository)
    {
        _logger = logger;
        _productTypeRepository = productTypeRepository;
        _context = context;
        _productTypeFilterRepository = productTypeFilterRepository;
        _filterCategoryRepository = filterCategoryRepository;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
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
