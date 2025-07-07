using NewsAggregator.Server.Dtos;
using NewsNotifier.Models.Entities;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<IEnumerable<User>> GetAllUsers();
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(int id);
    Task<bool> SaveArticle(int userId, int articleId);
    Task<List<SavedArticleDto>> GetSavedArticles(int userId);
    Task<bool> UnsaveArticle(int userId, int articleId);
}
