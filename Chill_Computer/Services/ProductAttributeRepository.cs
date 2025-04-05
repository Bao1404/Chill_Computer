using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly ChillComputerContext _context;
        public ProductAttributeRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<ProductAttribute> GetAllProductAttributes()
        {
            return _context.ProductAttributes.ToList();
        }
        public List<ProductAttribute> GetProductAttributeByProductId(int id)
        {
            var list = from productAttribute in _context.ProductAttributes join product in _context.Products
                       on productAttribute.ProductId equals product.ProductId
                       where productAttribute.ProductId == product.ProductId
                       select productAttribute;
            return list.ToList();
        }
    }
}
