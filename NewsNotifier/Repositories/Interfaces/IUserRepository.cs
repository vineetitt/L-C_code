using NewsAggregator.Server.Dtos;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> SaveArticleAsync(int userId, int articleId);
        Task<List<SavedArticleDto>> GetSavedArticlesAsync(int userId);

        Task<bool> UnsaveArticleAsync(int userId, int articleId);
    }
}
