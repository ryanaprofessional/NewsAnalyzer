namespace Ai.Models.Other.News
{
    public class NewsArticles : OperationalStaus
    {
        public int Id { get; set; } = -1;
        public Article[] Articles { get; set; }
    }
}