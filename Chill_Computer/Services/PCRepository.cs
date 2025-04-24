using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Services
{
    public class PCRepository : IPCRepository
    {
        private readonly ChillComputerContext _context;
        public PCRepository(ChillComputerContext context)
        {
            _context = context;
        }

        public List<Product> GetListProductsFromPCID(int pcId)
        {
            var products = _context.PcComponents.Include(pc => pc.Product)
                .Where(pc => pc.PcId == pcId)
                .Select(pc => pc.Product)
                .ToList();
            return products;
        }

        public List<Pc> GetPCs()
        {
            var pcs = _context.Pcs.ToList();
            return pcs;
        }

        public void AddPc(Pc pc)
        {
            _context.Pcs.Add(pc);
            _context.SaveChanges();
        }

        public void UpdatePc(Pc pc)
        {
            _context.Pcs.Update(pc);
            _context.SaveChanges();
        }

        public void DeletePc(int pcId)
        {
            var pc = _context.Pcs.Find(pcId);
            if (pc != null)
            {
                _context.Pcs.Remove(pc);
                _context.SaveChanges();
            }
        }
    }
}
