namespace NewsAggregator.Server.Dtos
{
    public class NewsArticleDto
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? URL { get; set; }
        public string? CategoryName { get; set; }
    }
}
