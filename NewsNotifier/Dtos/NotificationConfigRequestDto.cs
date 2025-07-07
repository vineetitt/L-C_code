using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Server.Dtos
{
    public class NotificationConfigRequestDto
    {
        [Required]
        public int CategoryID { get; set; }
        public string? Keywords { get; set; }
        public bool IsEnabled { get; set; }
    }
}
