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
            var clientTickets = await _context.Clienttickets
                .Where(t => t.Client == clientId).ToListAsync();
            
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
            if (ticket == null)
                return null;
            
            var trip = await _context.Trips
                .Include(t => t.Route)
                .FirstOrDefaultAsync(t => t.TripId == ticket.TripId);

            if (trip == null)
                return null;
            
            var ticketInfo = new TicketInfoDTO
            {
                Id = ticket.TicketId,
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
            var responce = await IssueTicket(model);

            if (responce.IsSuccess)
            {
                var ticketToBook = await _context.Tickets
                    .FindAsync(int.Parse(responce.Message));
                
                if (ticketToBook == null)
                    return new ServiceResponce
                    {
                        IsSuccess = false,
                        Message = "Ticket not found"
                    };
            
                ticketToBook.TriggerState(Ticket.Trigger.Book);
                ticketToBook.Status = "booked";
            
                _context.Tickets.Update(ticketToBook);

                await _context.SaveChangesAsync();

                return new ServiceResponce
                {
                    IsSuccess = true,
                    Message = ticketToBook.TicketId.ToString()
                };
            }

            return responce;
        }
        public async Task<ServiceResponce> BuyTicket(TicketDTO model)
        {
            var responce = await IssueTicket(model);

            if (responce.IsSuccess)
            {
                SetPaymentTimer(int.Parse(responce.Message));
                return new ServiceResponce
                {
                    IsSuccess = true,
                    Message = responce.Message
                };
            }

            return responce;
        }

        public async Task<ServiceResponce> CancelBooking(int ticketId) 
        {
            var ticketToCancel = await _context.Tickets.FindAsync(ticketId);

            if (ticketToCancel == null)
                return new ServiceResponce 
                {
                    IsSuccess = false,
                    Message = "Ticket not found"
                };

            ticketToCancel.TriggerState(Ticket.Trigger.Cancel);
            ticketToCancel.Status = "cancelled";
            _context.Tickets.Update(ticketToCancel);

            var seat = await _context.Seats.FindAsync(ticketToCancel.SeatId);
            if (seat == null)
                return new ServiceResponce
                {
                    IsSuccess = false,
                    Message = "Seat not found"
                };
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
                    IsSuccess = false,
                    Message = "Ticket not found"
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

        public async Task<ServiceResponce> IssueTicket(TicketDTO model)
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
                return new ServiceResponce
                {
                    IsSuccess = false,
                    Message = "Seat not found"
                };

            seatToBook.Available = false;
            _context.Seats.Update(seatToBook);
            
            var newTicket = new Ticket
            {
                DateTime = DateTime.Now,
                Status = "issued",
                PassengerId = passenger.PassengerId,
                SeatId = model.Seat,
                TripId = model.Trip
            };
            
            var clientId = GetClientIdFromToken();

            if (clientId == null)
                return new ServiceResponce
                {
                    IsSuccess = false,
                    Message = "ClientId not found"
                };
            
            await _context.Clienttickets.AddAsync(new Clientticket
            {
                Client = clientId,
                Ticket = newTicket.TicketId
            });

            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();
            
            return new ServiceResponce
            {
                IsSuccess = true,
                Message = newTicket.TicketId.ToString()
            };
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
        
        private void SetPaymentTimer(int ticketId)
        {
            int paymentTime = 60 * 5 * 1000;
            Timer paymentTimer = new Timer(async state => 
                    await CancelPayment((int)state), ticketId, 
                (long)paymentTime, Timeout.Infinite);
        }

        private async Task CancelPayment(int ticketId)
        {
            using (var context = new AutoContext())
            {
                var ticketToCancel = await context.Tickets.FindAsync(ticketId);
                if (ticketToCancel != null && ticketToCancel.Status.Equals("issued"))
                {
                    ticketToCancel.TriggerState(Ticket.Trigger.Cancel);
                    ticketToCancel.Status = "cancelled";
                    context.Tickets.Update(ticketToCancel);
                    var seat = await context.Seats.FindAsync(ticketToCancel.SeatId);
                    seat.Available = true;
                    context.Seats.Update(seat);

                    await context.SaveChangesAsync();
                }
            }
        }
        
    }
}
