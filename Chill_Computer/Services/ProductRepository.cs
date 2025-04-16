using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

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

        

        public IEnumerable<Product> GetProductsByTypeName(string typeName)
        {
            var list = from product in _context.Products
                       join typeProduct in _context.ProductTypes
                       on product.TypeId equals typeProduct.TypeId
                       where typeProduct.TypeName == typeName
                       select product;
            return list.ToList();
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

        public List<Product> GetProductByBrandId(int id)
        {
            var list = from product in _context.Products
                       join brand in _context.Brands
                       on product.BrandId equals brand.BrandId
                       where product.BrandId == brand.BrandId
                       select product;
            return list.ToList();
        }

        public List<Product> GetProductFromFilter(string filterName, string categoryName)
        {
            var list = (from pa in _context.ProductAttributes
                        join p in _context.Products on pa.ProductId equals p.ProductId
                        join a in _context.Attributes on pa.AttributeId equals a.AttributeId
                        join pt in _context.ProductTypes on a.TypeId equals pt.TypeId
                        join ptf in _context.ProductTypeFilters on pt.TypeId equals ptf.TypeId
                        join f in _context.FilterCategories on ptf.FilterId equals f.FilterId
                        where a.AttributeName.Contains(filterName) &&
                              pa.AttributeValue.Contains(categoryName)
                        select p).Distinct();

            return list.ToList();
        }
    }
}
