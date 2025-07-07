using System.ComponentModel.DataAnnotations;

namespace NewsNotifier.Models.Entities
{
    public class ExternalServer
    {
        [Key]
        public int ServerID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string APIKey { get; set; } = null!;

        public DateTime? LastAccessed { get; set; }

        public string? Status { get; set; }
    }
}
