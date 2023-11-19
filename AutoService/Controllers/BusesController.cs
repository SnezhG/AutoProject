using Microsoft.AspNetCore.Mvc;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;

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
        
        [HttpGet("BusSeats/{id}")]
        public async Task<ActionResult<IEnumerable<Seat>>> GetBusSeats(int id)
        {

            var seats = await _busService.GetBusSeats(id);

            if (seats == null)
                return NotFound();

            return Ok(seats);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus(int id, [FromBody] BusDTO model)
        {

            var result = await _busService.PutBus(id, model);

            if (result.IsSuccess)
                return Ok();

            return NotFound();
        }


        [HttpPost]
        public async Task<ActionResult> PostBus([FromBody] BusDTO model)
        {
            var result = await _busService.PostBus(model);

            if (result.IsSuccess)
                return Ok();

            return NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var result = await _busService.DeleteBus(id);

            if (result.IsSuccess)
                return Ok();

            return NotFound(result);
        }
    }
}
