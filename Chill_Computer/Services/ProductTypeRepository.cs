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
            var productTypes = _context.ProductTypes.ToList();

            return productTypes;
        }

        public ProductType GetProductTypeById(int id)
        {
            return _context.ProductTypes.FirstOrDefault(pt => pt.TypeId == id);
        }
    }
}
