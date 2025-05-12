using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PollChatApi.Service;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PollChatApi.Service
{
    public class WeeklyPollBackgroundService : BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public WeeklyPollBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    // Create a new DI scope
                    using var scope = _scopeFactory.CreateScope();
                    var pollHandler = scope.ServiceProvider.GetRequiredService<IPollHandler>();

                    await pollHandler.CreateWeeklyPollAsync();
                }

                // Wait an hour before checking again
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

    }
}
