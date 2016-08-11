using System;
using System.ComponentModel.DataAnnotations;

namespace MyTravel.ViewModels
{
    public class TripViewModel
    {
        [Required, StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
