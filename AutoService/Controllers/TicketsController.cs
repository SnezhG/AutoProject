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
    public class TicketsController : ControllerBase
    {
        private readonly AutoContext _context;

        public TicketsController(AutoContext context)
        {
            _context = context;
        }


        
        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
          if (_context.Tickets == null)
          {
              return NotFound();
          }
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
          if (_context.Tickets == null)
          {
              return NotFound();
          }
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        //Создание билета
/*        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
          if (_context.Tickets == null)
          {
              return Problem("Entity set 'AutoContext.Tickets'  is null.");
          }
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }*/

        [HttpPost("BookTicket")]
        public async Task<ActionResult> BookTicket([FromBody]TicketViewModel model)
        {
            if (ModelState.IsValid)
            {

                var passenger = new Passenger
                {
                    Surname = model.LastName,
                    Name = model.Name,
                    Patronimyc = model.Patronymic,
                    Sex = model.Sex,
                    DateOfBirth = model.DateOfBirth,
                    PassportNum = model.PassNum,
                    PassportSeries = model.PassSeries
                };

                var seatToBook = await _context.Seats.FindAsync(model.Seat);

                if (seatToBook == null)
                    return NotFound("Seat is not found");

                seatToBook.Available = false;
                _context.Seats.Update(seatToBook);

                var tripToBook = await _context.Trips.FindAsync(model.Trip);

                if (tripToBook == null)
                    return NotFound("Trip is not found");

                var ticketToBook = new Ticket
                {
                    DateTime = DateTime.Now,
                    Status = "Забронирован",
                    Passenger = passenger,
                    Seat = seatToBook,
                    Trip = tripToBook
                };

                await _context.Tickets.AddAsync(ticketToBook);
                await _context.SaveChangesAsync();

                return Ok("Ticket created");
            }

            return BadRequest("Some properties are incorrect");
        }

        [HttpPost("BuyTicket")]
        public async Task<ActionResult<Ticket>> BuyTicket(Ticket ticket)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'AutoContext.Tickets'  is null.");
            }

            ticket.DateTime = DateTime.Now;
            ticket.Status = "Оплачен";
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var seat = await _context.Seats.FindAsync(ticket.SeatId);
            seat.Available = false;
            _context.Seats.Update(seat);

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }

        [HttpPut("CancelBooking")]
        public async Task<ActionResult<Ticket>> CancelBooking(Ticket ticket)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'AutoContext.Tickets'  is null.");
            }
            ticket.Status = "Бронирование отменено";
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            var seat = await _context.Seats.FindAsync(ticket.SeatId);
            seat.Available = true;
            _context.Seats.Update(seat);

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.TicketId == id)).GetValueOrDefault();
        }
    }
}
