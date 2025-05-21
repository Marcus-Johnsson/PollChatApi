namespace PollChatApi.Service.Background
{
    public class DeleteFromDatabaseService : BackgroundService
    {
        private readonly IServiceScopeFactory _scope;

        public DeleteFromDatabaseService(IServiceScopeFactory scope) 
        {
            _scope = scope;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scope.CreateAsyncScope();
                var action = scope.ServiceProvider.GetRequiredService<DeleteFromDatabase>();

                await action.DeleteDailyFromDatabase();
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
