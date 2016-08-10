using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyTravel.Models
{
    public class TravelContext : DbContext
    {
        public TravelContext ()
        {
          
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }
    }
}