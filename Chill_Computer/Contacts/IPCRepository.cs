using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface IPCRepository
    {
        public List<Product> GetListProductsFromPCID(int pcId);
        public List<Pc> GetPCs();
        public void AddPc(Pc pc);
        public void UpdatePc(Pc pc);
        public void DeletePc(int pcId);
    }
}
