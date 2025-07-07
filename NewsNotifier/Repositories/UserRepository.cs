using Microsoft.EntityFrameworkCore;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using NewsAggregator.Server.Repositories.Interfaces;
namespace NewsNotifier.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveArticleAsync(int userId, int articleId)
        {
            var article = await _context.NewsArticles.FindAsync(articleId);
            if (article == null)
                return false;

            var alreadySaved = await _context.SavedArticles
                .AnyAsync(sa => sa.UserID == userId && sa.ArticleID == articleId);

            if (alreadySaved)
                return false;

            var savedArticle = new SavedArticle
            {
                UserID = userId,
                ArticleID = articleId,
                SavedDate = DateTime.UtcNow
            };

            _context.SavedArticles.Add(savedArticle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetSavedArticlesAsync(int userId)
        {
            return await _context.SavedArticles
                .Where(sa => sa.UserID == userId)
                .Include(sa => sa.NewsArticle)
                .Select(sa => new
                {
                    sa.NewsArticle.ArticleID,
                    sa.NewsArticle.Title,
                    sa.NewsArticle.Description,
                    sa.NewsArticle.URL,
                    sa.SavedDate
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<bool> UnsaveArticleAsync(int userId, int articleId)
        {
            var savedArticle = await _context.SavedArticles
                .FirstOrDefaultAsync(sa => sa.UserID == userId && sa.ArticleID == articleId);

            if (savedArticle == null)
                return false;

            _context.SavedArticles.Remove(savedArticle);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
