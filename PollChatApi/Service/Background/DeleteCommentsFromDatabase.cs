using Microsoft.Identity.Client;

namespace PollChatApi.Service.Background
{
    public class DeleteCommentsFromDatabase : BackgroundService
    {
        private readonly IServiceScopeFactory _scope;
        public DeleteCommentsFromDatabase(IServiceScopeFactory scope) 
        {
            _scope = scope;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scope.CreateAsyncScope();
                var action = scope.ServiceProvider.GetRequiredService<DeleteFromDatabase>();

                await action.DeleteDailyFromDataBaseComments();
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
