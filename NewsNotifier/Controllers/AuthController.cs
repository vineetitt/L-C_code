using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsAggregator.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService , IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            var result = await _authService.SignupAsync(request);
            if (result == "Email already exists")
                return BadRequest(result);

            await _emailService.SendEmailAsync(request.Email, "Welcome to News Aggregator",
        $"Hello {request.Username},<br/><br/>Welcome to the News Aggregator application!<br/><br/>Enjoy the latest news updates.<br/><br/>Happy Reading!<br/>");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var (token, role, userid) = await _authService.LoginAsync(request);
                return Ok(new
                { 
                    token, 
                    role,
                    userId = userid

                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }

}
