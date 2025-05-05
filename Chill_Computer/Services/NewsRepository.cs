using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Services
{
    public class NewsRepository : INewsRepository
    {
        private readonly ChillComputerContext _context;

        public NewsRepository(ChillComputerContext context)
        {
            _context = context;
        }

        public News ReadNews(int idNew)
        {
            return _context.News.FirstOrDefault(n => n.NewsId == idNew);
        }
    }
}
