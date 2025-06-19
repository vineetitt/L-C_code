using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("NewsArticle")]
        public int ArticleID { get; set; }
        public NewsArticle NewsArticle { get; set; } = null!;

        public DateTime SentDate { get; set; }
    }
}
