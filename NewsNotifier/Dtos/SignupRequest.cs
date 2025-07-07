using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Server.Dtos
{
    public class SignupRequest
    {
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*]).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter and one special character (!@#$%^&*)")]
        public string Password { get; set; }
    }
}
