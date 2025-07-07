using NewsAggregator.Server.Interfaces;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;
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

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public Task<IEnumerable<User>> GetAllUsersAsync() => _userRepository.GetAllUsersAsync();

        public Task CreateUserAsync(User user) => _userRepository.AddUserAsync(user);

        public Task UpdateUserAsync(User user) => _userRepository.UpdateUserAsync(user);

        public Task DeleteUserAsync(int id) => _userRepository.DeleteUserAsync(id);


        public async Task<bool> SaveArticleAsync(int userId, int articleId)
        {
            return await _userRepository.SaveArticleAsync(userId, articleId);
        }

        public async Task<List<object>> GetSavedArticlesAsync(int userId)
        {
            return await _userRepository.GetSavedArticlesAsync(userId);
        }

        public async Task<bool> UnsaveArticleAsync(int userId, int articleId)
        {
            return await _userRepository.UnsaveArticleAsync(userId, articleId);
        }
    }
}
