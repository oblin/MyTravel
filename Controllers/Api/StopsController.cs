using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTravel.Models;
using MyTravel.ViewModels;

namespace MyTravel
{
    [Route("api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ITravelRepository _repository;
        private ILogger<StopsController> _logger;

        public StopsController(ITravelRepository repository, ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(
                    trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");
            }
            return BadRequest("Failed to get Stops");
        }

        [HttpPost]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(model);
                    _repository.AddStop(tripName, newStop);
                    
                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                            Mapper.Map<StopViewModel>(newStop));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new Stop: {0}", ex.Message);
            }

            return BadRequest("Failed to saven new Stop");
        }
    }
}