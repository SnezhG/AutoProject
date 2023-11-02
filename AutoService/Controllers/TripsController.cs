using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;
using AutoService.ViewModels;
using AutoService.Services;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly TripsService _tripsService;

        public TripsController(TripsService tripsService)
        {
            _tripsService = tripsService;
        }


        [HttpGet("FindTrips")]
        public async Task<ActionResult<IEnumerable<Trip>>> FindTrips([FromBody] FindTripViewModel model)
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
        public async Task<IActionResult> PutTrip(int id, [FromBody] TripViewModel model)
        {
            var result = await _tripsService.PutTrip(id, model);
            if(result.IsSuccess)
                return Ok();
            return NotFound();
        }


        [HttpPost]
        public async Task<ActionResult> PostTrip([FromBody] TripViewModel model)
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
