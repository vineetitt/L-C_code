using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        
        public string Name { get; set; } = null!;

        public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public ICollection<NotificationConfig> NotificationConfigs { get; set; } = new List<NotificationConfig>();
    }
}
