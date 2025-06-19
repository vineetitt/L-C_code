using NewsNotifier.Models.Entities;

namespace NewsNotifier.Dtos.External
{
    public class NewsApiResponse
    {
        public string Status { get; set; } = null!;
        public int TotalResults { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
    }

    public class Article
    {
        public Source Source { get; set; } = new Source();
        public string? Author { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Content { get; set; }
    }

    public class Source
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
