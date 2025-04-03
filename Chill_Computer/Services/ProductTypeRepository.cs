using Chill_Computer.Models;
using Chill_Computer.Services;

namespace Chill_Computer.Contacts
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ChillComputerContext _context;
        public ProductTypeRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<ProductType> GetProductTypes()
        {
            var productTypes = _context.ProductTypes
                .Join(_context.ProductTypeFilters,
                    type => type.TypeId,
                    typeFilter => typeFilter.TypeId,
                    (type, typeFilter) => new { ProductType = type, ProductTypeFilter = typeFilter })
                .Join(_context.FilterCategories,
                    tf => tf.ProductTypeFilter.FilterId,
                    category => category.FilterId,
                    (tf, category) => new { ProductType = tf.ProductType })
                .Select(result => result.ProductType)
                .ToList();

            return productTypes;
        }


    }
}
