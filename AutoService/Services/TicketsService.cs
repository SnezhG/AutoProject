using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutoService.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly AutoContext _context;

        public TicketsService(AutoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTickets(string clientId) 
        {
            var ticketsId = await _context.Clienttickets.Where(t => t.Client == clientId).ToListAsync();
            if (ticketsId == null)
                return null;

            var tickets = new List<Ticket>();
            foreach (var ticketId in ticketsId) 
            {
                tickets.Add(await _context.Tickets.FindAsync(ticketId));
            }

            return tickets;
        }
        public async Task<Ticket> GetTicket(int id) 
        {
            return await _context.Tickets.FindAsync(id);
        }
        public async Task<ServiceResponce> BookTicket(TicketViewModel model)
        {
            var ticketToBookId = await IssueTicket(model);
            if (ticketToBookId == -1)
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            var ticketToBook = await _context.Tickets.FindAsync(ticketToBookId);
            if (ticketToBook == null)
                return new ServiceResponce
                {
                    IsSuccess = false
                };
            
            ticketToBook.TriggerState(Ticket.Trigger.Book);
            ticketToBook.Status = "booked";
            
            _context.Tickets.Update(ticketToBook);

            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> BuyTicket(TicketViewModel model)
        {
            var ticketToBuyId = await IssueTicket(model);
            if (ticketToBuyId == -1)
                return new ServiceResponce
                {
                    IsSuccess = false
                };
            
            var result = await PayForTicket(ticketToBuyId);

            if(result.IsSuccess)
                return new ServiceResponce
                {
                    IsSuccess = true
                };
            
            return new ServiceResponce
            {
                IsSuccess = false
            };
        }
        public async Task<ServiceResponce> CancelBooking(int ticketId) 
        {
            var ticketToCancel = await _context.Tickets.FindAsync(ticketId);

            if (ticketToCancel == null)
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            ticketToCancel.TriggerState(Ticket.Trigger.Cancel);
            ticketToCancel.Status = "canceled";
            _context.Tickets.Update(ticketToCancel);

            var seat = await _context.Seats.FindAsync(ticketToCancel.SeatId);
            seat.Available = true;
            _context.Seats.Update(seat);

            await _context.SaveChangesAsync();

            return new ServiceResponce
            {
                IsSuccess = true
            };
        }

        public async Task<ServiceResponce> PayForTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            ticket.Status = "paid";
            ticket.DateTime = DateTime.Now;
            _context.Tickets.Update(ticket);
            
            return new ServiceResponce
            {
                IsSuccess = true
            };
        }

        public async Task<int> IssueTicket(TicketViewModel model)
        {
            var checkPassenger = await _context.Passengers.FirstOrDefaultAsync(p =>
                p.PassportNum == model.PassNum && p.PassportSeries == model.PassSeries
            );

            Passenger passenger;

            if (checkPassenger == null)
            {
                passenger = new Passenger
                {
                    Surname = model.LastName,
                    Name = model.Name,
                    Patronimyc = model.Patronymic,
                    Sex = model.Sex,
                    DateOfBirth = model.DateOfBirth,
                    PassportNum = model.PassNum,
                    PassportSeries = model.PassSeries
                };

                await _context.Passengers.AddAsync(passenger);
            }
            else
            {
                passenger = checkPassenger;
            }

            var seatToBook = await _context.Seats.FindAsync(model.Seat);

            if (seatToBook == null)
                return -1;

            seatToBook.Available = false;
            _context.Seats.Update(seatToBook);

            var newTicket = new Ticket
            {
                Status = "issued",
                Passenger = passenger,
                SeatId = model.Seat,
                TripId = model.Trip
            };

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();
            
            /*await _context.Clienttickets.AddAsync(new Clientticket
            {
                Client = model.clientId,
                Ticket = newTicket.TicketId
            });*/
            
            return newTicket.TicketId;
        }
    }
}
