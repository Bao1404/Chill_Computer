using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IBrandRepository
    {
        List<Brand> GetAllBrands();
        public Brand GetBrandNameById(int id);
    }
}
