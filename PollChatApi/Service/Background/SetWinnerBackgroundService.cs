using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PollChatApi.Service.Background
{
    public class SetWinnerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SetWinnerBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var pollHandler = scope.ServiceProvider.GetRequiredService<SettWinner>();

                    await pollHandler.SetWinner();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
