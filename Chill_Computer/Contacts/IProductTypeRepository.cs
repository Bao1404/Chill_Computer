using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public interface IProductTypeRepository
    {
        List<ProductType> GetProductTypes();
    }
}
