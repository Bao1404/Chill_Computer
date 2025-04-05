using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ChillComputerContext _context;
        public ProductRepository(ChillComputerContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> GetProductByTypeId(int id)
        {
            var list = from product in _context.Products
                       join typeProduct in _context.ProductTypes
                       on product.TypeId equals typeProduct.TypeId
                       where product.TypeId == typeProduct.TypeId
                       select product;
            return list.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public List<string> GetProductVersionFromProductName(string name)
        {
            var list = from product in _context.Products
                       where product.ProductName == name
                       select product.Version;
            return list.ToList();
        }
    }
}
