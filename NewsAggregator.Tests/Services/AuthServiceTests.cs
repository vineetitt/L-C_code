using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using NewsAggregator.Server.Services;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsAggregator.Server.Dtos;
using NewsNotifier.Models.Entities;
using System;
using System.Threading.Tasks;

namespace Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<IAuthRepository> _authRepoMock = new();
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
        private readonly Mock<IConfiguration> _configMock = new();

        public AuthServiceTests()
        {
            _authService = new AuthService(_authRepoMock.Object, _passwordHasherMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var request = new LoginRequest { Email = "vineet@gmail.com", Password = "Vineet@123" };
            var user = new User { UserID = 1, Email = request.Email, PasswordHash = "hashed-password", Role = "User", Username = "Vineet" };

            _authRepoMock.Setup(x => x.GetUserByEmailAsync(request.Email))
                         .ReturnsAsync(user);

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, request.Password))
                               .Returns(PasswordVerificationResult.Success);

            _configMock.Setup(x => x["Jwt:Key"]).Returns("super-secret-key-1234567890123456");

            // Act
            var (token, role, userId) = await _authService.LoginAsync(request);

            // Assert
            token.Should().NotBeNullOrEmpty();
            role.Should().Be("User");
            userId.Should().Be(1);
        }

        [Fact]
        public async Task Login_ShouldThrowUnauthorized_WhenPasswordIsInvalid()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "wrong-password" };
            var user = new User { UserID = 1, Email = request.Email, PasswordHash = "hashed-password" };

            _authRepoMock.Setup(x => x.GetUserByEmailAsync(request.Email))
                         .ReturnsAsync(user);

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, request.Password))
                               .Returns(PasswordVerificationResult.Failed);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(request));
        }

        [Fact]
        public async Task Login_ShouldThrowUnauthorized_WhenUserDoesNotExist()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "password123" };

            _authRepoMock.Setup(x => x.GetUserByEmailAsync(request.Email))
                         .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(request));
        }


        [Fact]
        public async Task Signup_ShouldReturnError_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new SignupRequest { Email = "vineet@gmail.com", Username = "Vineet", Password = "Vineet@1234" };
            var existingUser = new User { UserID = 1, Email = request.Email };

            _authRepoMock.Setup(x => x.GetUserByEmailAsync(request.Email))
                         .ReturnsAsync(existingUser);

            // Act
            var result = await _authService.SignupAsync(request);

            // Assert
            result.Should().Be("Email already exists");
        }

        [Fact]
        public async Task Signup_ShouldRegisterUserSuccessfully_WhenEmailIsUnique()
        {
            // Arrange
            var request = new SignupRequest { Email = "unique@example.com", Username = "UniqueUser", Password = "password123" };

            _authRepoMock.Setup(x => x.GetUserByEmailAsync(request.Email))
                         .ReturnsAsync((User)null);

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
                               .Returns("hashed-password");

            _authRepoMock.Setup(x => x.AddUserAsync(It.IsAny<User>()))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.SignupAsync(request);

            // Assert
            result.Should().Be("User registered successfully");
        }

    }
}
