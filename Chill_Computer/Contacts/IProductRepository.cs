using Chill_Computer.Models;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Contacts
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        public List<Product> GetProductPagination(int pageNumber, int pageSize);
        List<Product> GetProductByTypeId(int id);
        public Product GetProductById(int id);
        List<string> GetProductVersionFromProductName(string name);
        List<Product> GetProductByBrandId(int id);
        public List<Product> GetProductsByName(string name);
        public List<Product> GetProductsByName(string name, int pageNumber, int pageSize);
        public IEnumerable<Product> GetProductsByTypeName(string typeName);
        public List<ManageProductViewModel> GetProducts(int pageNumber, int pageSize);
        public void AddProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);
        List<Product> GetProductFromFilter(string filterName, string categoryName);

        public Product GetProductFromNameAndVersion(string productName, string versionName);
    }
}
