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

        public AuthService(IAuthRepository authRepository, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        public async Task<string> SignupAsync(SignupRequest request)
        {
            var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                return "Email already exists";

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = "User"
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _authRepository.AddUserAsync(user);
            return "User registered successfully";
        }

        public async Task<(string Token, string Role, int userId)> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = GenerateJwtToken(user);
            return (token, user.Role, user.UserID);
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
