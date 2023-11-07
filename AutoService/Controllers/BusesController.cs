using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AutoService.Services;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusesController(IBusService busService)
        {
            _busService = busService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
        {
            var buses = await _busService.GetBusesAsync();

            if (buses == null)
                return NotFound();

            return Ok(buses);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Bus>> GetBus(int id)
        {

            var bus = await _busService.GetBus(id);

            if (bus == null)
                return NotFound();

            return Ok(bus);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus(int id, [FromBody] BusViewModel model)
        {

            var result = await _busService.PutBus(id, model);

            if (result.IsSuccess)
                return Ok();

            return NotFound();
        }


        [HttpPost]
        public async Task<ActionResult> PostBus([FromBody] BusViewModel model)
        {
          if (ModelState.IsValid)
          {
                var result = await _busService.PostBus(model);

                if (result.IsSuccess)
                    return Ok();

                return BadRequest();
          };

            return BadRequest("Some properties are incorrect");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var result = await _busService.DeleteBus(id);

            if (result.IsSuccess)
                return Ok();

            return NotFound();
        }
    }
}
