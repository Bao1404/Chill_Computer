using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IAttributeRepository
    {
        List<Models.Attribute> GetAllAttributes();
        List<Models.Attribute> GetAttributeByTypeId(int id);
    }
}
