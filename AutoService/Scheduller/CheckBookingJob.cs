﻿using AutoService.Data;
using AutoService.Models;
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
            var ticketsToCheck = await _context.Tickets
                .Where(ticket =>
                ticket.Status == "booked")
                .ToListAsync();

            if (ticketsToCheck != null)
            {
                foreach(var tick in ticketsToCheck) 
                {
                    if (DateTime.Now > tick.DateTime.AddMinutes(1)) 
                    {
                        tick.TriggerState(Ticket.Trigger.Expire);
                        tick.Status = "expired";
                        _context.Tickets.Update(tick);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
