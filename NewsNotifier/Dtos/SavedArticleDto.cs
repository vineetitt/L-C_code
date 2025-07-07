namespace NewsAggregator.Server.Dtos
{
    public class SavedArticleDto
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public int CategoryID { get; set; }
        public DateTime SavedDate { get; set; }
    }
}
