using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class SavedArticle
    {
        [Key]
        public int SavedID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("NewsArticle")]
        public int ArticleID { get; set; }
        public NewsArticle NewsArticle { get; set; } = null!;

        public DateTime SavedDate { get; set; }
    }
}
