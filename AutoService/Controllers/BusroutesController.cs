using Microsoft.AspNetCore.Mvc;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusroutesController : ControllerBase
    {
        private readonly IBusRoutesService _busroutesService;

        public BusroutesController(IBusRoutesService busRoutesService)
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
        public async Task<IActionResult> PutBusroute(int id, [FromBody] BusRouteDTO model)
        {
            var result = await _busroutesService.PutBusroute(id, model);

            if (result.IsSuccess)
                return Ok();

            return NotFound(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostBusroute([FromBody] BusRouteDTO model)
        {
            var result = await _busroutesService.PostBusroute(model);
            if (result.IsSuccess)
                return Ok();
            return NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusroute(int id)
        {
            var result = await _busroutesService.DeleteBusroute(id);
            if (result.IsSuccess)
                return Ok();
            return NotFound(result);
        }
    }
}
