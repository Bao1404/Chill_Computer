using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface INewsRepository
    {
        public News ReadNews(int idNew);

    }
}
