using AutoService.Data;
using System.Text.RegularExpressions;

namespace AutoService.Services
{
    public class BookingCheckService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AutoContext _context;

        public BookingCheckService(IServiceProvider serviceProvider, AutoContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public Task StartAsync(CancellationToken stoppingToken) 
        {
            Task.Run(() => CheckTickets());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingTokken) 
        {
            return Task.CompletedTask;
        }

        private void CheckTickets()
        {
            var bookedTickets = _context.Tickets
                .Where(ticket => ticket.Status == "Забронирован")
                .ToList();

            foreach (var ticket in bookedTickets) 
            {
                DateTime dateTimeValue = (DateTime)ticket.DateTime;

                if (DateTime.Now > dateTimeValue.AddMinutes(1)) 
                {
                    ticket.Status = "Бронирование истекло";
                    _context.Tickets.Update(ticket);
                }
            }
        }
    }
}
