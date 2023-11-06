﻿using AutoService.Data;
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

        public async Task<IEnumerable<Ticket>> GetTickets(int clientId) 
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
            var ticketToBook = await IssueTicket(model);

            ticketToBook.TriggerState(Ticket.Trigger.Book);

            ticketToBook.Status = "booked";
            
            await _context.Tickets.AddAsync(ticketToBook);

            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> BuyTicket(TicketViewModel model)
        {
            var ticketToBuy = await IssueTicket(model);
            await _context.Tickets.AddAsync(ticketToBuy);
            var result = await PayForTicket(ticketToBuy.TicketId);

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
            /*
            ticket.Status = "paid";

            ticket.TriggerState(Ticket.Trigger.Pay);
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            */

            return new ServiceResponce
            {
                IsSuccess = true
            };
        }

        public async Task<Ticket> IssueTicket(TicketViewModel model)
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
                return null;

            seatToBook.Available = false;
            _context.Seats.Update(seatToBook);

            var ticketToBook = new Ticket
            {
                DateTime = DateTime.Now,
                Status = "issued",
                Passenger = passenger,
                SeatId = model.Seat,
                TripId = model.Trip
            };

            await _context.Clienttickets.AddAsync(new Clientticket
            {
                Client = model.clientId,
                Ticket = ticketToBook.TicketId
            });

            return ticketToBook;
        }
    }
}
