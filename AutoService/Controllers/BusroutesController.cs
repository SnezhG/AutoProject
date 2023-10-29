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
    public class BusroutesController : ControllerBase
    {
        private readonly AutoContext _context;

        public BusroutesController(AutoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Busroute>>> GetBusroutes()
        {
          if (_context.Busroutes == null)
          {
              return NotFound();
          }
            return await _context.Busroutes.ToListAsync();
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusroute(int id, [FromBody] BusRouteViewModel model)
        {
            var routeToEdit = await _context.Busroutes.FindAsync(id);

            if (routeToEdit == null)
                return NotFound("Cannot find the route");

            routeToEdit.ArrCity = model.ArrCity;
            routeToEdit.DepCity = model.DepCity;


            _context.Busroutes.Update(routeToEdit);

            await _context.SaveChangesAsync();

            return Ok("Route info updated successfuly");
        }

        [HttpPost]
        public async Task<ActionResult> PostBusroute([FromBody] BusRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var routeToCreate = new Busroute
                {
                    ArrCity = model.ArrCity,
                    DepCity = model.DepCity
                };

                _context.Busroutes.Add(routeToCreate);
                await _context.SaveChangesAsync();

                return Ok("Route created successfully");
            };

            return BadRequest("Route properties are incorrect");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusroute(int id)
        {
            var routeToDelete = await _context.Busroutes.FindAsync(id);
            if (routeToDelete == null)
                return NotFound("Route not found");

            _context.Busroutes.Remove(routeToDelete);
            await _context.SaveChangesAsync();

            return Ok("Route deleted successfuly");
        }
    }
}
