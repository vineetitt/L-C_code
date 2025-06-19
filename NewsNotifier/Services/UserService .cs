using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;

namespace NewsNotifier.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetUserByIdAsync(int id) => _userRepository.GetUserByIdAsync(id);

        public Task<IEnumerable<User>> GetAllUsersAsync() => _userRepository.GetAllUsersAsync();

        public Task CreateUserAsync(User user) => _userRepository.AddUserAsync(user);

        public Task UpdateUserAsync(User user) => _userRepository.UpdateUserAsync(user);

        public Task DeleteUserAsync(int id) => _userRepository.DeleteUserAsync(id);
    }
}
