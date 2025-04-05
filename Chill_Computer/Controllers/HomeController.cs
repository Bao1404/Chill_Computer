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

    public HomeController(ILogger<HomeController> logger, ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository)
        : base(context, productTypeRepository, productTypeFilterRepository)
    {
        _logger = logger;
        _productTypeRepository = productTypeRepository;
        _context = context;
        _productTypeFilterRepository = productTypeFilterRepository;
    }

    public IActionResult Index()
    {
        Init();
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
