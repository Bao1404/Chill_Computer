using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IFilterCategoryRepository
    {
        List<FilterCategory> GetAllCategory();
        public List<FilterCategory> GetGetCategoriesByFilterId(int id);
    }
}
