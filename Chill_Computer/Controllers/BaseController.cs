using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Chill_Computer.Controllers
{
    public class BaseController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly ChillComputerContext _context;
        private readonly IProductTypeFilterRepository _productTypeFilterRepository;
        private readonly IFilterCategoryRepository _filterCategoryRepository;
        public BaseController(ChillComputerContext context, IProductTypeRepository productTypeRepository, IProductTypeFilterRepository productTypeFilterRepository, IFilterCategoryRepository filterCategoryRepository)
        {
            _context = context;
            _productTypeRepository = productTypeRepository;
            _productTypeFilterRepository = productTypeFilterRepository;
            _filterCategoryRepository = filterCategoryRepository;
        }
        public IActionResult Init()
        {
            List<ProductTypeFilter> filters = new List<ProductTypeFilter>();
            List<FilterCategory> categories = new List<FilterCategory>();
            foreach(var typeFilter in _productTypeRepository.GetProductTypes())
            {
                filters.AddRange(_productTypeFilterRepository.GetByProductTypeId(typeFilter.TypeId));
                foreach(var category in _productTypeFilterRepository.GetByProductTypeId(typeFilter.TypeId))
                {
                    categories.AddRange(_filterCategoryRepository.GetGetCategoriesByFilterId(category.FilterId));
                }
            }
            ViewBag.MenuTitle = _productTypeRepository.GetProductTypes();
            ViewBag.FilterTitle = filters;
            ViewBag.Categories = categories;
            return View();
        }
    }
}
