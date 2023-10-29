using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AutoService.Services
{
    public static class DependencyInjection
    {
        public static void ScheduleJob(this IServiceCollection services) 
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(CheckBookingJob));

                options
                    .AddJob<CheckBookingJob>(jobKey)
                    .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobKey)
                            .WithSimpleSchedule(schedule =>
                                schedule.WithIntervalInSeconds(60).RepeatForever()));
            });

            services.AddQuartzHostedService();
        }

    }
}
