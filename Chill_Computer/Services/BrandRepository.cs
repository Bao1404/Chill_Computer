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
        public void AddBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }

        public Brand GetBrandByName(string name)
        {
            if (ExistBrandName(name))
            {
                Brand br = _context.Brands.FirstOrDefault(x => x.BrandName.Equals(name));
                return br;
            }
            return null;
        }

        public bool ExistBrandName(string brandName)
        {
            return _context.Brands.Any(b => b.BrandName == brandName);
        }
    }
}
