using Microsoft.AspNetCore.Mvc;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }


        [HttpPost("FindTrips")]
        public async Task<ActionResult<List<TripInfoDTO>>> FindTrips([FromBody] FindTripDTO model)
        {
            if (ModelState.IsValid)
            {
                var tripsFiltered = await _tripsService.FindTrips(model);
                if (tripsFiltered == null)
                    return NotFound();

                return Ok(tripsFiltered);
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            var trip = await _tripsService.GetTrip(id);
            if (trip == null)
                return NotFound();
            return Ok(trip);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(int id, [FromBody] TripDTO model)
        {
            var result = await _tripsService.PutTrip(id, model);
            if(result.IsSuccess)
                return Ok();
            return NotFound();
        }


        [HttpPost]
        public async Task<ActionResult> PostTrip([FromBody] TripDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tripsService.PostTrip(model);
                if (result.IsSuccess)
                    return Ok();
                return NotFound();
            };

            return BadRequest("Some properties are incorrect");
        }
    }
}
