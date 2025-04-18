using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IBrandRepository
    {
        List<Brand> GetAllBrands();
        public Brand GetBrandNameById(int id);
        public Brand GetBrandByName(string name);
        public void AddBrand(Brand brand);
    }
}
