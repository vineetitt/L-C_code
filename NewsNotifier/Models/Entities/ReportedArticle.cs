using NewsNotifier.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Server.Models.Entities
{
    public class ReportedArticle
    {
        [Key]
        public int ReportID { get; set; }

        [ForeignKey("NewsArticle")]
        public int ArticleID { get; set; }
        public NewsArticle NewsArticle { get; set; } = null!;

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    }
}
