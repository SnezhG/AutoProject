using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly AutoContext _context;

        public TicketsService(AutoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTickets() 
        {
            return await _context.Tickets.ToListAsync();
        }
        public async Task<Ticket> GetTicket(int id) 
        {
            return await _context.Tickets.FindAsync(id);
        }
        public async Task<ServiceResponce> BookTicket(TicketViewModel model) 
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
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            seatToBook.Available = false;
            _context.Seats.Update(seatToBook);

            var ticketToBook = new Ticket
            {
                DateTime = DateTime.Now,
                Status = "Забронирован",
                Passenger = passenger,
                SeatId = model.Seat,
                TripId = model.Trip
            };

            await _context.Tickets.AddAsync(ticketToBook);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> BuyTicket(TicketViewModel model) 
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
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            seatToBook.Available = false;
            _context.Seats.Update(seatToBook);

            var ticketToBook = new Ticket
            {
                DateTime = DateTime.Now,
                Status = "Оплачен",
                Passenger = passenger,
                SeatId = model.Seat,
                TripId = model.Trip
            };

            await _context.Tickets.AddAsync(ticketToBook);
            await _context.SaveChangesAsync();

            return new ServiceResponce
            {
                IsSuccess = true
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

            ticketToCancel.Status = "Бронирование отменено";
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
            return new ServiceResponce
            {
                IsSuccess = false
            };
        }
        public async Task<ServiceResponce> DeleteTicket(int id) 
        {
            var ticketToDelete = await _context.Buses.FindAsync(id);

            if (ticketToDelete == null)
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            _context.Buses.Remove(ticketToDelete);
            await _context.SaveChangesAsync();

            return new ServiceResponce
            {
                IsSuccess = true
            };
        }
    }
}
