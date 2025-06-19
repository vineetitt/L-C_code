namespace NewsAggregator.Server.Models.Entities
{
    public class ExternalApiConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string Status { get; set; } // Active/Inactive
        public DateTime? LastAccessed { get; set; }
    }
}
