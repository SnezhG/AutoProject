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

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly AutoContext _context;

        public TripsController(AutoContext context)
        {
            _context = context;
        }


        [HttpGet("FindTrips")]
        public async Task<ActionResult> FindTrips([FromBody] FindTripViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tripsFiltered = await _context.Trips.Where(trip =>
                    trip.DepTime == model.DepDate && 
                    trip.Route.DepCity == model.DepCity &&
                    trip.Route.ArrCity == model.ArrCity).ToListAsync();

                return Ok(tripsFiltered);
            }

            return NotFound("Cant find trips");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
          if (_context.Trips == null)
          {
              return NotFound();
          }
            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(int id, [FromBody] TripViewModel model)
        {
            var tripToEdit = await _context.Trips.FindAsync(id);

            if (tripToEdit == null)
                return NotFound("Cannot find the trip");

            tripToEdit.DepTime = model.DepTime;
            tripToEdit.ArrTime = model.ArrTime;
            tripToEdit.BusId = model.BusId;
            tripToEdit.RouteId = model.RouteId;
            tripToEdit.DriverId = model.DriverId;
            tripToEdit.ConductorId = model.CondId;
            tripToEdit.Price = model.Price;

            _context.Trips.Update(tripToEdit);

            await _context.SaveChangesAsync();

            return Ok("Trip info updated successfuly");
        }


        [HttpPost]
        public async Task<ActionResult> PostTrip([FromBody] TripViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tripToCreate = new Trip
                {
                    DepTime = model.DepTime,
                    ArrTime = model.ArrTime,
                    BusId = model.BusId,
                    RouteId = model.RouteId,
                    DriverId = model.DriverId,
                    ConductorId = model.CondId,
                    Price = model.Price
                };

                var bus = await _context.Buses.FindAsync(model.BusId);

                if (bus == null)
                    return NotFound("Couldnt find bus");

                var driver = await _context.Personnel.FindAsync(model.DriverId);

                if (driver == null)
                    return NotFound("Couldnt find driver");

                var conductor = await _context.Personnel.FindAsync(model.CondId);

                if (conductor == null)
                    return NotFound("Couldnt find conductor");

                bus.Available = false;
                driver.Available = false;
                conductor.Available = false;

                _context.Buses.Update(bus);
                _context.Personnel.Update(driver);
                _context.Personnel.Update(conductor);

                _context.Trips.Add(tripToCreate);

                await _context.SaveChangesAsync();

                return Ok("Trip created successfully");
            };

            return BadRequest("Some properties are incorrect");
        }
    }
}
