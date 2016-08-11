using System.Collections.Generic;
using System.Linq;

namespace MyTravel.Models
{
    public interface ITravelRepository
    {
        IEnumerable<Trip> GetAllTrips();
    }

    public class TravelRepository : ITravelRepository
    {
        private TravelContext _context;

        public TravelRepository (TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }
    }
}