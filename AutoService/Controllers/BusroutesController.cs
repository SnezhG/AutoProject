using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusroutesController : ControllerBase
    {
        private readonly AutoContext _context;

        public BusroutesController(AutoContext context)
        {
            _context = context;
        }

        // GET: api/Busroutes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Busroute>>> GetBusroutes()
        {
          if (_context.Busroutes == null)
          {
              return NotFound();
          }
            return await _context.Busroutes.ToListAsync();
        }

        // GET: api/Busroutes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Busroute>> GetBusroute(int id)
        {
          if (_context.Busroutes == null)
          {
              return NotFound();
          }
            var busroute = await _context.Busroutes.FindAsync(id);

            if (busroute == null)
            {
                return NotFound();
            }

            return busroute;
        }

        // PUT: api/Busroutes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusroute(int id, Busroute busroute)
        {
            if (id != busroute.RouteId)
            {
                return BadRequest();
            }

            _context.Entry(busroute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusrouteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Busroutes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Busroute>> PostBusroute(Busroute busroute)
        {
          if (_context.Busroutes == null)
          {
              return Problem("Entity set 'AutoContext.Busroutes'  is null.");
          }
            _context.Busroutes.Add(busroute);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusroute", new { id = busroute.RouteId }, busroute);
        }

        // DELETE: api/Busroutes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusroute(int id)
        {
            if (_context.Busroutes == null)
            {
                return NotFound();
            }
            var busroute = await _context.Busroutes.FindAsync(id);
            if (busroute == null)
            {
                return NotFound();
            }

            _context.Busroutes.Remove(busroute);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusrouteExists(int id)
        {
            return (_context.Busroutes?.Any(e => e.RouteId == id)).GetValueOrDefault();
        }
    }
}
