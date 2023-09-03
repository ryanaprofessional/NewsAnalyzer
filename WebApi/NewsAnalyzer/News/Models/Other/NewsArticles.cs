namespace News.Models.Other
{
    public class NewsArticles : OperationalStaus
    {
        public int Id { get; set; } = -1;
        public string Code { get; set; } = string.Empty;
        public Article[] Articles { get; set; }
    }
}