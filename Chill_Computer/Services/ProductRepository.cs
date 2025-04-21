using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.ViewModels;
using Microsoft.EntityFrameworkCore;
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

        public List<ManageProductViewModel> GetProducts(int pageNumber, int pageSize)
        {
            return (from product in _context.Products
                    join type in _context.ProductTypes on product.TypeId equals type.TypeId
                    join brand in _context.Brands on product.BrandId equals brand.BrandId
                    join serie in _context.Series on product.SeriesId equals serie.SeriesId into seriesGroup
                    from serie in seriesGroup.DefaultIfEmpty()
                    select new ManageProductViewModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductVersion = product.Version ?? string.Empty,
                        ProductType = type.TypeName,
                        ProductBrand = brand.BrandName,
                        TypeId = product.TypeId ?? 0,
                        BrandId = product.BrandId ?? 0,
                        Price = product.Price,
                        FormattedPrice = product.FormattedPrice ?? string.Empty,
                        Stock = product.Stock,
                        SeriesId = product.SeriesId ?? 0
                    })
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }

        public List<Product> GetProductPagination(int pageNumber, int pageSize)
        {
            return _context.Products
                .Include(p => p.Type)
                .Include(p => p.Brand)
                .Include(p => p.Series)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
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

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<Product> GetProductsByName(string name)
        {
            return _context.Products.Where(p => p.ProductName == name).ToList();
        }

        public List<Product> GetProductsByName(string name, int pageNumber, int pageSize)
        {
            return _context.Products
                     .Include(p => p.Type)
                     .Include(p => p.Brand)
                     .Include(p => p.Series)
                     .Where(p => p.ProductName.Contains(name))
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();
        }

        public List<Product> GetProductFromFilter(string filterName, string categoryName)
        {
            var list = from pa in _context.ProductAttributes
                       join p in _context.Products on pa.ProductId equals p.ProductId
                       join a in _context.Attributes on pa.AttributeId equals a.AttributeId
                       join pt in _context.ProductTypes on a.TypeId equals pt.TypeId
                       join ptf in _context.ProductTypeFilters on pt.TypeId equals ptf.TypeId
                       join f in _context.FilterCategories on ptf.FilterId equals f.FilterId
                       where a.AttributeName.Contains(filterName) && pa.AttributeValue.Contains(categoryName)
                       select p;
            return list.Distinct().ToList();
        }

        public Product GetProductFromNameAndVersion(string productName, string versionName)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductName == productName && p.Version == versionName);
            return product;
        }


    }
}
