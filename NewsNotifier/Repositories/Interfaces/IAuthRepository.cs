using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}
