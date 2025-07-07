using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Dtos.ServerDtos;

namespace NewsAggregator.Server.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync()
        {
            return await _context.ExternalServers
                .Select(server => new ServerSummaryDto
                {
                    ServerId = server.ServerID,
                    Name = server.Name,
                    Status = server.Status
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ExternalServer>> GetAllServersAsync()
        {
            return await _context.ExternalServers.ToListAsync();
        }

        public async Task<ExternalServer?> GetServerByIdAsync(int serverId)
        {
            return await _context.ExternalServers.FindAsync(serverId);
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

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(int newsId)
        {
            return await _context.NewsArticles.FindAsync(newsId);
        }

        public async Task UpdateNewsAsync(NewsArticle news)
        {
            _context.NewsArticles.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsAsync(NewsArticle news)
        {
            _context.NewsArticles.Remove(news);
            await _context.SaveChangesAsync();
        }
    }
}
