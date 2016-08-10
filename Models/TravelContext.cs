using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyTravel.Models
{
    public class TravelContext : DbContext
    {
        private IConfigurationRoot _config;

        public TravelContext (IConfigurationRoot config, DbContextOptions options)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_config["ConnectionStrings:Postgresql"]);
        }
    }
}