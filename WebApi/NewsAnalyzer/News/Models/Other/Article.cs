using Amazon.DynamoDBv2.DataModel;

namespace News.Models.Other
{
    public class Article
    {
        [DynamoDBProperty("source")]
        public ArticleSource? Source { get; set; } = null;
        [DynamoDBProperty("author")]
        public string Author { get; set; } = string.Empty;
        [DynamoDBProperty("title")]
        public string Title { get; set; } = string.Empty;
        [DynamoDBProperty("description")]
        public string Description { get; set; } = string.Empty;
        [DynamoDBProperty("url")]
        public string Url { get; set; } = string.Empty;
        [DynamoDBProperty("urlToImage")]
        public string UrlToImage { get; set; } = string.Empty;
        [DynamoDBProperty("publishedAt")]
        public string PublishedAt { get; set; } = string.Empty;
        [DynamoDBProperty("content")]
        public string Content { get; set; } = string.Empty;
    }
}
