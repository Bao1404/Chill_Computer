using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class FilterCategoryRepository : IFilterCategoryRepository
    {
        private readonly ChillComputerContext _context;

        public FilterCategoryRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<FilterCategory> GetAllCategory()
        {
            return _context.FilterCategories.ToList();
        }
        public List<FilterCategory> GetGetCategoriesByFilterId(int filterId)
        {
            var categoryName = from filter in _context.ProductTypeFilters join category in _context.FilterCategories
                               on filter.FilterId equals category.FilterId
                               where filter.FilterId == filterId
                               select category;
            return categoryName.ToList();
        }
    }
}
