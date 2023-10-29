using AutoService.Data;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace AutoService.Services
{
    public class CheckBookingJob : IJob
    {
        private readonly ILogger<CheckBookingJob> _logger;
        private readonly AutoContext _context;

        public CheckBookingJob(ILogger<CheckBookingJob> logger, AutoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("job being done {UtcNow}", DateTime.UtcNow);

            var ticketsToCheck = await _context.Tickets.Where(ticket =>
                ticket.Status == "Забронирован").ToListAsync();

            foreach(var tick in ticketsToCheck) 
            {
                if (DateTime.Now > tick.DateTime.AddMinutes(1)) 
                {
                    tick.Status = "Бронирование отменено";
                    _context.Tickets.Update(tick);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
