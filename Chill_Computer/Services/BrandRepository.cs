using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ChillComputerContext _context;
        public BrandRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<Brand> GetAllBrands()
        {
            return _context.Brands.ToList();
        }
        public Brand GetBrandNameById(int id)
        {
            return _context.Brands.FirstOrDefault(b => b.BrandId == id);
        }
    }
}
