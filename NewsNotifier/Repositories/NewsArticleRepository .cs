using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Models.Entities;

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
            var blockedKeywords = await _context.BlockedKeywords
                .Select(k => k.Keyword)
                .ToListAsync();

            return await _context.NewsArticles
                .Include(a => a.Category)
                .Where(a => !a.IsHidden && !a.Category.IsHidden &&
                    !blockedKeywords.Any(k => a.Title.ToLower().Contains(k) || (a.Content ?? "").ToLower().Contains(k)))
                .ToListAsync();

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

        public async Task ReportArticleAsync(ReportedArticle report)
        {
            _context.ReportedArticles.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetReportCountAsync(int articleId)
        {
            return await _context.ReportedArticles.CountAsync(r => r.ArticleID == articleId);
        }

        public async Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync()
        {
            return await _context.ReportedArticles
                .Include(r => r.NewsArticle) 
                .GroupBy(r => r.ArticleID)
                .Select(g => new
                {
                    Article = g.First().NewsArticle, 
                    ReportCount = g.Count()
                })
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(x => (x.Article, x.ReportCount)));
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task AddBlockedKeywordAsync(BlockedKeyword keyword)
        {
            _context.BlockedKeywords.Add(keyword);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetBlockedKeywordsAsync()
        {
            return await _context.BlockedKeywords
                .Select(k => k.Keyword)
                .ToListAsync();
        }

        public async Task DeleteBlockedKeywordAsync(string keyword)
        {
            var existing = await _context.BlockedKeywords
                .FirstOrDefaultAsync(k => k.Keyword == keyword);
            if (existing != null)
            {
                _context.BlockedKeywords.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<NewsArticle>> GetPersonalizedNewsAsync(List<int> categoryIds, List<string> keywords)
        {
            return await _context.NewsArticles
                .Include(a => a.Category)
                .Where(a => !a.IsHidden && !a.Category.IsHidden &&
                       (categoryIds.Contains(a.CategoryID) ||
                       keywords.Any(k => a.Title.ToLower().Contains(k) || (a.Content ?? "").ToLower().Contains(k))))
                .ToListAsync();
        }


    }
}
