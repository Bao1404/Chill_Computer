using Chill_Computer.Models;

namespace Chill_Computer.Contacts
{
    public interface ISeriesRepository
    {
        public List<Series> GetAllSeries();
        public Series GetSeriesById(int id);
        public Series GetSeriesByName(string name);
        public void AddSeries(Series series);
        public void UpdateSeries(Series series);
        public void DeleteSeries(int id);
        public List<Series> GetSeriesByBrandId(int brandId);
    }
}
