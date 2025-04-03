using Chill_Computer.Models;
using Chill_Computer.Services;
using Microsoft.EntityFrameworkCore;

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

        public ProductTypeFilter GetById(int id)
        {
            return _context.ProductTypeFilters.FirstOrDefault(filter => filter.TypeId == id);
        }
    }
}

