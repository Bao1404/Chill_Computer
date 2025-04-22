using Chill_Computer.Contacts;
using Chill_Computer.Models;

namespace Chill_Computer.Services
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly ChillComputerContext _context;

        public SeriesRepository(ChillComputerContext context)
        {
            _context = context;
        }
        public List<Series> GetAllSeries()
        {
            return _context.Series.ToList();
        }
        public Series GetSeriesById(int id)
        {
            return _context.Series.Find(id);
        }
        public void AddSeries(Series series)
        {
            _context.Series.Add(series);
            _context.SaveChanges();
        }
        public void UpdateSeries(Series series)
        {
            _context.Series.Update(series);
            _context.SaveChanges();
        }
        public void DeleteSeries(int id)
        {
            var series = GetSeriesById(id);
            if (series != null)
            {
                _context.Series.Remove(series);
                _context.SaveChanges();
            }
        }

        public Series GetSeriesByName(string name)
        {
            if (ExistSeriesName(name))
            {
                return _context.Series.FirstOrDefault(a => a.SeriesName == name);
            }
            return null;
        }

        public bool ExistSeriesName(string seriesName)
        {
            return _context.Series.Any(b => b.SeriesName == seriesName);
        }

        public List<Series> GetSeriesByBrandId(int brandId)
        {
            return _context.Series.Where(s => s.BrandId == brandId).ToList();
        }
    }
    
}
