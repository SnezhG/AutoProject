using System.IdentityModel.Tokens.Jwt;
using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Transactions;
using Microsoft.IdentityModel.Tokens;

namespace AutoService.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly AutoContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;

        public TicketsService(AutoContext context, IHttpContextAccessor httpContext, IConfiguration configuration)
        {
            _context = context;
            _httpContext = httpContext;
            _configuration = configuration;
        }

        public async Task<List<TicketInfoDTO>> GetTickets()
        {
            var clientId = GetClientIdFromToken();
            var clientTickets = await _context.Clienttickets.Where(t => t.Client == clientId).ToListAsync();
            if (clientTickets == null)
                return null;

            var tickets = new List<TicketInfoDTO>();
            foreach (var ticket in clientTickets)
            {
                var ticketInfo = await GetTicket(ticket.Ticket);
                tickets.Add(ticketInfo);
            }

            return tickets;
        }
        public async Task<TicketInfoDTO> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Seat)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(t => t.TicketId == id);
            
            var trip = await _context.Trips
                .Include(t => t.Route)
                .FirstOrDefaultAsync(t => t.TripId == ticket.TripId);

            var ticketInfo = new TicketInfoDTO
            {
                Name = ticket.Passenger.Name,
                Surname = ticket.Passenger.Surname,
                Patronymic = ticket.Passenger.Patronimyc,
                DepCity = trip.Route.DepCity,
                ArrCity = trip.Route.ArrCity,
                DepDate = DateOnly.FromDateTime(trip.DepTime),
                ArrDate = DateOnly.FromDateTime(trip.ArrTime),
                DepTime = trip.DepTime.ToShortTimeString(),
                ArrTime = trip.ArrTime.ToShortTimeString(),
                Seat = ticket.Seat.Num,
                Price = ticket.Trip.Price,
                Status = ticket.Status
            };
            
            return ticketInfo;
        }
        public async Task<ServiceResponce> BookTicket(TicketDTO model)
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
        public async Task<int> BuyTicket(TicketDTO model)
        {
            var ticketToBuyId = await IssueTicket(model);
            /*if (ticketToBuyId == -1)
                return new ServiceResponce
                {
                    IsSuccess = false
                };*/
            return ticketToBuyId;
            /*var result = await PayForTicket(ticketToBuyId);

            if(result.IsSuccess)
                return new ServiceResponce
                {
                    IsSuccess = true
                };*/

            /*return new ServiceResponce
            {
                IsSuccess = false
            };*/
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
            ticket.TriggerState(Ticket.Trigger.Pay);
            ticket.Status = "paid";
            ticket.DateTime = DateTime.Now;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return new ServiceResponce
            {
                IsSuccess = true
            };
        }

        public async Task<int> IssueTicket(TicketDTO model)
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
                await _context.SaveChangesAsync();
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
                PassengerId = passenger.PassengerId,
                SeatId = model.Seat,
                TripId = model.Trip
            };

            /*await _context.Tickets.AddAsync(newTicket);
            var clientId = GetClientIdFromToken();
            if (clientId == null)
                return -1;
            await _context.Clienttickets.AddAsync(new Clientticket
            {
                Client = clientId,
                Ticket = newTicket.TicketId
            });*/

            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();
            
            return newTicket.TicketId;
        }

        private string GetClientIdFromToken()
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
            var authHeader = _httpContext.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            var claimsPrincipal = handler.ValidateToken(token, validationParameters, out var validatedToken);
            var clientId = claimsPrincipal.Claims.FirstOrDefault(c =>
                c.Type == "ClientId")?.Value;
            return clientId;
        }


    }
}
