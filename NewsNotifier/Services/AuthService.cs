using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Interfaces;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsAggregator.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;

        private const string DefaultUserRole = "User";
        private const string InvalidCredentialsMessage = "Invalid credentials";

        public AuthService(IAuthRepository authRepository, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        public async Task<string> SignupAsync(SignupRequest request)
        {
            if (await EmailAlreadyExists(request.Email))
                return "Email already exists";

            var newUser = CreateUser(request);

            await _authRepository.AddUserAsync(newUser);

            return "User registered successfully";
        }

        public async Task<(string Token, string Role, int userId)> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !VerifyPassword(user, request.Password))
                throw new UnauthorizedAccessException(InvalidCredentialsMessage);

            var token = GenerateJwtToken(user);

            return (token, user.Role, user.UserID);
        }

        private async Task<bool> EmailAlreadyExists(string email)
        {
            var existingUser = await _authRepository.GetUserByEmailAsync(email);
            return existingUser != null;
        }

        private User CreateUser(SignupRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = DefaultUserRole,
                PasswordHash = string.Empty 
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            return user;
        }

        private bool VerifyPassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result != PasswordVerificationResult.Failed;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.UserID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
