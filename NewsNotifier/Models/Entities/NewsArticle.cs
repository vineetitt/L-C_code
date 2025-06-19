using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class NewsArticle
    {
        [Key]
        public int ArticleID { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public string? Source { get; set; }

        public string? URL { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string ?Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; } = null!;

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public ICollection<SavedArticle> SavedArticles { get; set; } = new List<SavedArticle>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
