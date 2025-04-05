using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly ChillComputerContext _context;
        public AttributeRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<Models.Attribute> GetAllAttributes()
        {
            return _context.Attributes.ToList();
        }

        public List<Models.Attribute> GetAttributeByTypeId(int id)
        {
            var list = from att in _context.Attributes
                       join type in _context.ProductTypes
                       on att.TypeId equals type.TypeId
                       where att.TypeId == type.TypeId
                       select att;
            return list.ToList();
        }
    }
}
