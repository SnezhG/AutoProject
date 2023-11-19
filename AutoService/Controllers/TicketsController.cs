using Microsoft.AspNetCore.Mvc;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketsService _ticketService;

        public TicketsController(ITicketsService ticketsService)
        {
            _ticketService = ticketsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketInfoDTO>>> GetTickets()
        {
            var tickets = await _ticketService.GetTickets();
            if (tickets == null)
                return NotFound();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketInfoDTO>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicket(id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }


        [HttpPost("BookTicket")]
        public async Task<IActionResult> BookTicket([FromBody]TicketDTO model)
        {
            var result = await _ticketService.BookTicket(model);
            if (result.IsSuccess)
                return Ok(int.Parse(result.Message));
            return NotFound(result);
        }

        [HttpPost("BuyTicket")]
        public async Task<IActionResult> BuyTicket([FromBody] TicketDTO model)
        {
            var result = await _ticketService.BuyTicket(model);
            if (result.IsSuccess)
                return Ok(int.Parse(result.Message));
            return NotFound(result);
        }

        [HttpPost("CancelBooking")]
        public async Task<IActionResult> CancelBooking([FromBody] int id)
        {
            var result = await _ticketService.CancelBooking(id);
            if (result.IsSuccess)
                return Ok();
            return NotFound(result);
        }

        [HttpPost("PayForTicket")]
        public async Task<IActionResult> PayForTicket([FromBody] int id) 
        {
            var result = await _ticketService.PayForTicket(id);
            if (result.IsSuccess)
                return Ok(int.Parse(result.Message));
            return NotFound(result);
        }
        
    }
}
