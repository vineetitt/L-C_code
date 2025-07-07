using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<NewsArticle> GetNews(int? categoryId)
        {
            var blockedKeywords = _context.BlockedKeywords.Select(k => k.Keyword.ToLower()).ToList();

            var query = _context.NewsArticles
                        .Where(n => !n.IsHidden &&
                                    !blockedKeywords.Any(k => n.Title.ToLower().Contains(k) || (n.Content ?? "").ToLower().Contains(k))).AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(n => n.CategoryID == categoryId.Value);
            }

            return query.ToList();
        }

        public NewsArticle? GetNewsById(int id)
        {
            return _context.NewsArticles
                .FirstOrDefault(n => n.ArticleID == id);
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
