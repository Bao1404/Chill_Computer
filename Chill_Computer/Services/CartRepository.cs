using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class CartRepository
    {
        private readonly ChillComputerContext _context;
        public CartRepository(ChillComputerContext context)
        {
            _context = context;
        }
    }
}
