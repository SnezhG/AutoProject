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
    public class BusroutesController : ControllerBase
    {
        private readonly BusRoutesService _busroutesService;

        public BusroutesController(BusRoutesService busRoutesService)
        {
            _busroutesService = busRoutesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Busroute>>> GetBusroutes()
        {
            var routes = await _busroutesService.GetBusroutes();
            if (routes == null)
                return NotFound();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Busroute>> GetBusroute(int id)
        {
            var route = await _busroutesService.GetBusroute(id);
            if(route == null) 
                return NotFound();
            return Ok(route);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusroute(int id, [FromBody] BusRouteViewModel model)
        {
            var result = await _busroutesService.PutBusroute(id, model);

            if (result.IsSuccess)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> PostBusroute([FromBody] BusRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _busroutesService.PostBusroute(model);
                if (result.IsSuccess)
                    return Ok();
                return BadRequest();
            };

            return BadRequest("Route properties are incorrect");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusroute(int id)
        {
            var result = await _busroutesService.DeleteBusroute(id);
            if (result.IsSuccess)
                return Ok();
            return NotFound();
        }
    }
}
