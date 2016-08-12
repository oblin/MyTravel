using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTravel.Models;
using MyTravel.ViewModels;

namespace MyTravel
{
    [Route("api/trips")]
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
            try{
            var result = _repository.GetAllTrips(); 
            return Ok(Mapper.Map<IEnumerable<TripViewModel>>(result));
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");
                return BadRequest($"Error Occurs: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TripViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(model);
                return Created($"api/trips/{model.Name}", Mapper.Map<TripViewModel>(newTrip));
            }
            return BadRequest(ModelState);
        }
    }
}
