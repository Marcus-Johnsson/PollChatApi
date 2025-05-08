using System.Diagnostics.Eventing.Reader;

namespace PollChatApi.Service
{
    public class WeeklyPollBackgroundService : BackgroundService
    {

        private readonly IPollService _pollService;

        public WeeklyPollBackgroundService(IPollService pollService)
        {
            _pollService = pollService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    await _pollService.CreateWeeklyPollAsync();

                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

    }
}
