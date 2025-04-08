using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IProductAttributeRepository
    {
        List<ProductAttribute> GetAllProductAttributes();
        List<ProductAttribute> GetProductAttributeByProductId(int id);
    }
}
