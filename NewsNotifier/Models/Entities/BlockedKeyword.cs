using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Server.Models.Entities
{
    public class BlockedKeyword
    {
        [Key]
        public int KeywordID { get; set; }

        [Required]
        public string Keyword { get; set; } = null!;
    }
}
