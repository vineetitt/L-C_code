using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories
{
    public class NotificationConfigRepository : INotificationConfigRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationConfigRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<NotificationConfig>> GetAllConfigsAsync()
        {
            return await _context.NotificationConfigs.Include(n => n.User).ToListAsync();
        }

        public async Task<List<NotificationConfig>> GetConfigsByUserIdAsync(int userId)
        {
            return await _context.NotificationConfigs
                .Where(n => n.UserID == userId)
                .Include(n => n.Category)
                .ToListAsync();
        }

        public async Task AddOrUpdateConfigAsync(NotificationConfig config)
        {
            var existingConfig = await _context.NotificationConfigs
                .FirstOrDefaultAsync(c => c.UserID == config.UserID && c.CategoryID == config.CategoryID);

            if (existingConfig != null)
            {
                existingConfig.IsEnabled = config.IsEnabled;
                existingConfig.Keywords = config.Keywords;
            }
            else
            {
                _context.NotificationConfigs.Add(config);
            }

            await _context.SaveChangesAsync();
        }


        public async Task SaveOrUpdateConfigAsync(NotificationConfig config)
        {
            var existingConfig = await _context.NotificationConfigs
                .FirstOrDefaultAsync(c => c.UserID == config.UserID && c.CategoryID == config.CategoryID);

            if (existingConfig != null)
            {
                existingConfig.Keywords = config.Keywords;
                existingConfig.IsEnabled = config.IsEnabled;
                _context.NotificationConfigs.Update(existingConfig);
            }
            else
            {
                _context.NotificationConfigs.Add(config);
            }

            await _context.SaveChangesAsync();
        }

    }
}
