namespace NewsAggregator.Server.Dtos
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
