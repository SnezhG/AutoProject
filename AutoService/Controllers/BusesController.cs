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
    public class BusesController : ControllerBase
    {
        private readonly AutoContext _context;

        public BusesController(AutoContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
        {
          if (_context.Buses == null)
          {
              return NotFound();
          }
            return await _context.Buses.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Bus>> GetBus(int id)
        {
          if (_context.Buses == null)
          {
              return NotFound();
          }
            var bus = await _context.Buses.FindAsync(id);

            if (bus == null)
            {
                return NotFound();
            }

            return bus;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus(int id, [FromBody] BusViewModel model)
        {
            var busToEdit = await _context.Buses.FindAsync(id);

            if (busToEdit == null)
                return NotFound("Cannot find the bus");

            busToEdit.SeatCapacity = model.SeatCap;
            busToEdit.Available = model.Avail;
            busToEdit.Model = model.Model;
            busToEdit.Specs = model.Specs;

            _context.Buses.Update(busToEdit);

            await _context.SaveChangesAsync();

            return Ok("Bus info updated successfuly");
        }


        [HttpPost]
        public async Task<ActionResult> PostBus([FromBody] BusViewModel model)
        {
          if (ModelState.IsValid)
          {
                var busToCreate = new Bus
                {
                    SeatCapacity = model.SeatCap,
                    Model = model.Model,
                    Specs = model.Specs,
                    Available = model.Avail
                };

                _context.Buses.Add(busToCreate);
                await _context.SaveChangesAsync();

                return Ok("Bus created successfully");
          };
            
            return BadRequest("Some properties are incorrect");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
                return NotFound("Bus not found");

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return Ok("Bus deleted successfuly");
        }
    }
}
