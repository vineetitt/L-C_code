using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewsNotifier.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NewsArticle?> GetByIdAsync(int id)
        {
            return await _context.NewsArticles.FindAsync(id);
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles.ToListAsync();
        }

        public async Task AddAsync(NewsArticle article)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle article)
        {
            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}
