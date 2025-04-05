using Chill_Computer.Models;
using Chill_Computer.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Chill_Computer.Contacts
{
    public class ProductTypeFilterRepository : IProductTypeFilterRepository
    {
        private readonly ChillComputerContext _context;
        public ProductTypeFilterRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<ProductTypeFilter> GetTypeFilters()
        {
            return _context.ProductTypeFilters.ToList();
        }

        public List<ProductTypeFilter> GetByProductTypeId(int typeId)
        {
            var filters = (from productType in _context.ProductTypes
                           join typeFilter in _context.ProductTypeFilters
                           on productType.TypeId equals typeFilter.TypeId
                           where productType.TypeId == typeId
                           select typeFilter)
                          .OrderBy(filter => filter.FilterId).ToList();
            return filters;
        }
    }
}

