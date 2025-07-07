using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Interfaces;

namespace NewsAggregator.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        private const string WelcomeSubject = "Welcome to News Aggregator";
        private const string WelcomeBodyTemplate = "Hello {0},<br/><br/>Welcome to the News Aggregator application!<br/><br/>Enjoy the latest news updates.<br/><br/>Happy Reading!<br/>";

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignupRequest request)
        {
            var result = await _authService.SignupAsync(request);

            if (result == "Email already exists")
                return BadRequest(new { message = result });

            var emailBody = string.Format(WelcomeBodyTemplate, request.Username);

            await _emailService.SendEmailAsync(request.Email, WelcomeSubject, emailBody);

            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var (token, role, userId) = await _authService.LoginAsync(request);

                return Ok(new
                {
                    token,
                    role,
                    userId
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
