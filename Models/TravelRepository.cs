using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MyTravel.Models
{
    public interface ITravelRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        IEnumerable<Trip> GetAllTripsWithStops(string name);
        Trip GetTripByName(string tripName);
        Trip GetTripByName(string tripName, string username);
        void AddTrip(Trip trip);
        void AddStop(string tripName, string username, Stop newStop);
        Task<bool> SaveChangesAsync();
    }

    public class TravelRepository : ITravelRepository
    {
        private TravelContext _context;
        private ILogger<TravelRepository> _logger;

        public TravelRepository(TravelContext context, ILogger<TravelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, string username, Stop newStop)
        {
            var trip = GetTripByName(tripName, username);
            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops(string name)
        {
            try
            {
                return _context.Trips
                        .Include(t => t.Stops)
                        .OrderBy(t => t.Name)
                        .Where(t => t.UserName == name)
                        .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                           .Include(t => t.Stops)
                           .FirstOrDefault(t => t.Name == tripName);
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips.Include(t => t.Stops)
                           .FirstOrDefault(t => t.Name == tripName && t.UserName == username);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}