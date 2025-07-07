using NewsAggregator.Server.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Repositories.Interfaces;

namespace NewsNotifier.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetUserByIdAsync(int id) => _userRepository.GetUserById(id);

        public Task<IEnumerable<User>> GetAllUsersAsync() => _userRepository.GetAllUsers();

        public Task CreateUserAsync(User user) => _userRepository.AddUser(user);

        public Task UpdateUserAsync(User user) => _userRepository.UpdateUser(user);

        public Task DeleteUserAsync(int id) => _userRepository.DeleteUser(id);

        public Task<bool> SaveArticleAsync(int userId, int articleId) => _userRepository.SaveArticle(userId, articleId);

        public Task<List<SavedArticleDto>> GetSavedArticlesAsync(int userId) => _userRepository.GetSavedArticles(userId);

        public Task<bool> UnsaveArticleAsync(int userId, int articleId) => _userRepository.UnsaveArticle(userId, articleId);
    }
}

