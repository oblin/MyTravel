using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTravel.Models;
using MyTravel.Services;
using MyTravel.ViewModels;

namespace MyTravel
{
    [Route("api/trips/{tripName}/stops")][Authorize]
    public class StopsController : Controller
    {
        private ITravelRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coordService;

        public StopsController(ITravelRepository repository,
            GeoCoordsService coordService,
            ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }

        [HttpGet]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName, User.Identity.Name);
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

                    var result = await _coordService.GetGoogleMapCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;
                        _repository.AddStop(tripName, User.Identity.Name, newStop);

                        if (await _repository.SaveChangesAsync())
                        {
                            var addr = System.Net.WebUtility.UrlEncode(newStop.Name);
                            return Created($"/api/trips/{tripName}/stops/{addr}",
                                Mapper.Map<StopViewModel>(newStop));
                        }
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
