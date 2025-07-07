using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        Task<bool> SaveArticleAsync(int userId, int articleId);
        Task<List<object>> GetSavedArticlesAsync(int userId);
        Task<bool> UnsaveArticleAsync(int userId, int articleId);

    }
}
