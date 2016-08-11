using Microsoft.AspNetCore.Mvc;
using MyTravel.Models;
using MyTravel.ViewModels;

namespace MyTravel
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITravelRepository _repository;

        public TripsController(ITravelRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllTrips());
        }

        [HttpPost]
        public IActionResult Post([FromBody] TripViewModel model)
        {
            if (ModelState.IsValid){
                return Created($"api/trips/{model.Name}", model);
            }
            return BadRequest(ModelState);
        }
    }
}
