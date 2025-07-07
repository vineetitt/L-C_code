using NewsAggregator.Server.Interfaces;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Services
{
    public class NotificationConfigService : INotificationConfigService
    {
        private readonly INotificationConfigRepository _repository;

        public NotificationConfigService(INotificationConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveOrUpdateConfigAsync(NotificationConfig config)
        {
            await _repository.SaveOrUpdateConfigAsync(config);
        }
    }
}
