using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        public ICollection<SavedArticle> SavedArticles { get; set; } = new List<SavedArticle>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<NotificationConfig> NotificationConfigs { get; set; } = new List<NotificationConfig>();
    }
}
