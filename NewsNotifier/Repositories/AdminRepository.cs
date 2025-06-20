using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NewsAggregator.Server.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExternalServer>> GetAllServersAsync()
        {
            return await _context.ExternalServers.ToListAsync();
        }

        public async Task<ExternalServer?> GetServerByIdAsync(int ServerId)
        {
            return await _context.ExternalServers.FindAsync(ServerId);
        }

        public async Task UpdateServerAsync(ExternalServer server)
        {
            _context.ExternalServers.Update(server);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }
    }
}
