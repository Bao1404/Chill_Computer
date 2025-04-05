using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        List<Product> GetProductByTypeId(int id);
        public Product GetProductById(int id);
        List<string> GetProductVersionFromProductName(string name);
    }
}
