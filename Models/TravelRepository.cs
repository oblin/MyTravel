using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MyTravel.Models
{
    public interface ITravelRepository
    {
        IEnumerable<Trip> GetAllTrips();
    }

    public class TravelRepository : ITravelRepository
    {
        private TravelContext _context;
        private ILogger<TravelRepository> _logger; 

        public TravelRepository (TravelContext context, ILogger<TravelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogWarning("Get All Trips from database");
            return _context.Trips.ToList();
        }
    }
}