using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTravel.Models;
using MyTravel.ViewModels;

namespace MyTravel
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private ITravelRepository _repository;
        private ILogger<TripsController> _logger;

        public TripsController(ITravelRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var trips = _repository.GetAllTripsWithStops(User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(trips));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");
            }
            return BadRequest($"Error Occurs when get all Trips");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TripViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(model);
                newTrip.UserName = User.Identity.Name;
                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    var encodedName = System.Net.WebUtility.UrlEncode(newTrip.Name);
                    return Created($"api/trips/{encodedName}", Mapper.Map<TripViewModel>(newTrip));
                }
                else
                    return BadRequest("Failed to save changes to the database");
            }
            return BadRequest(ModelState);
        }
    }
}
