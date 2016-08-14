using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyTravel.ViewModels
{
    public class TripViewModel
    {
        [Required, StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public ICollection<StopViewModel> Stops { get; set; }
    }
}
