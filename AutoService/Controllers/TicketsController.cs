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
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var tickets = await _ticketService.GetTickets();
            if (tickets == null)
                return NotFound();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicket(id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }


        [HttpPost("BookTicket")]
        public async Task<IActionResult> BookTicket([FromBody]TicketDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _ticketService.BookTicket(model);
                if (result.IsSuccess)
                    return Ok();
                return NotFound();
            }

            return BadRequest("Some properties are incorrect");
        }

        [HttpPost("BuyTicket")]
        public async Task<IActionResult> BuyTicket([FromBody] TicketDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _ticketService.BookTicket(model);
                if (result.IsSuccess)
                    return Ok();
                return NotFound();
            }

            return BadRequest("Some properties are incorrect");
        }

        [HttpPut("CancelBooking")]
        public async Task<IActionResult> CancelBooking(int ticketId)
        {
            var result = await _ticketService.CancelBooking(ticketId);
            if (result.IsSuccess)
                return Ok();
            return NotFound();
        }

        [HttpPut("PayForTicket")]
        public async Task<IActionResult> PayForTicket(int id) 
        {
            var result = await _ticketService.PayForTicket(id);
            if (result.IsSuccess)
                return Ok();
            return NotFound();
        }
        
    }
}
