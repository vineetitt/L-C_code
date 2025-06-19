using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class NotificationConfig
    {
        [Key]
        public int ConfigID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; } = null!;

        public string? Keywords { get; set; }

        public bool IsEnabled { get; set; }
    }
}
