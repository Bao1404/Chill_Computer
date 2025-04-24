using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class PcComponentRepository : IPcComponentRepository
    {
        private readonly ChillComputerContext _context;
        public PcComponentRepository(ChillComputerContext context)
        {
            _context = context;
        }

        public void AddListProductToPC (int pcId, List<int> productIds)
        {
            foreach (var productId in productIds)
            {
                var pcComponent = new PcComponent
                {
                    PcId = pcId,
                    ProductId = productId
                };
                _context.PcComponents.Add(pcComponent);
            }
            _context.SaveChanges();
        }
    }
}
