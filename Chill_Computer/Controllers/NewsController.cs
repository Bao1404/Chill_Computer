using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chill_Computer.Controllers
{
    public class NewsController : BaseController
    {
        private readonly ChillComputerContext _context;
        private readonly INewsRepository _newsRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public NewsController(ChillComputerContext context, INewsRepository newsRepository, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository) :
            base(context, productTypeRepository, productTypeFilterRepository, filterCategoryRepository, cartRepository, cartItemRepository)
        {
            _context = context;
            _newsRepository = newsRepository;
            _productTypeRepository = productTypeRepository;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult ReadNew(int id)
        {
            Init();
            var news = _newsRepository.ReadNews(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);

        }

    }
}
