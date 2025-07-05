using NewsNotifier.Data;
using NewsNotifier.Interfaces;

namespace NewsNotifier.Services
{
    public class NewsFetchBackgroundService : BackgroundService
    {
        private readonly ILogger<NewsFetchBackgroundService> _logger;
        private readonly INewsApiService _newsApiService;

        private readonly IServiceScopeFactory _scopeFactory;

        public NewsFetchBackgroundService(IServiceScopeFactory scopeFactory, ILogger<NewsFetchBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var newsApiService = scope.ServiceProvider.GetRequiredService<INewsApiService>();

                    try
                    {
                        await newsApiService.FetchAndSaveNewsAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while fetching news.");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(3), stoppingToken); 
            }
        }
    }
}
