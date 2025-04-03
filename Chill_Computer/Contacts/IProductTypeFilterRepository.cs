using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public interface IProductTypeFilterRepository
    {
        List<ProductTypeFilter> GetTypeFilters();
        ProductTypeFilter GetById(int id);
    }
}
