using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyTravel.Models
{
    public class TravelUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}