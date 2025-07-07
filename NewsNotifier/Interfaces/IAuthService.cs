using NewsAggregator.Server.Dtos;

namespace NewsAggregator.Server.Interfaces
{
    public interface IAuthService
    {
        Task<string> SignupAsync(SignupRequest request);
        Task<(string Token, string Role, int userId)> LoginAsync(LoginRequest request);
    }
}
