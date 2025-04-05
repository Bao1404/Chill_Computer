using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public interface IProductTypeFilterRepository
    {
        List<ProductTypeFilter> GetTypeFilters();
        List<ProductTypeFilter> GetByProductTypeId(int typeId);
    }
}
