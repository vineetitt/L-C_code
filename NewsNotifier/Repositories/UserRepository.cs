using Microsoft.EntityFrameworkCore;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsAggregator.Server.Dtos;

namespace NewsNotifier.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveArticle(int userId, int articleId)
        {
            if (!await ArticleExistsAsync(articleId))
                return false;

            if (await IsArticleAlreadySaved(userId, articleId))
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

        public async Task<List<SavedArticleDto>> GetSavedArticles(int userId)
        {
            return await _context.SavedArticles
                .Where(sa => sa.UserID == userId)
                .Include(sa => sa.NewsArticle)
                .Select(sa => new SavedArticleDto
                {
                    ArticleID = sa.NewsArticle.ArticleID,
                    Title = sa.NewsArticle.Title,
                    Description = sa.NewsArticle.Description,
                    URL = sa.NewsArticle.URL,
                    CategoryID = sa.NewsArticle.CategoryID,
                    SavedDate = sa.SavedDate
                })
                .ToListAsync();
        }

        public async Task<bool> UnsaveArticle(int userId, int articleId)
        {
            var savedArticle = await _context.SavedArticles
                .FirstOrDefaultAsync(sa => sa.UserID == userId && sa.ArticleID == articleId);

            if (savedArticle == null)
                return false;

            _context.SavedArticles.Remove(savedArticle);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> ArticleExistsAsync(int articleId)
        {
            return await _context.NewsArticles.AnyAsync(a => a.ArticleID == articleId);
        }

        private async Task<bool> IsArticleAlreadySaved(int userId, int articleId)
        {
            return await _context.SavedArticles
                .AnyAsync(sa => sa.UserID == userId && sa.ArticleID == articleId);
        }
    }
}
