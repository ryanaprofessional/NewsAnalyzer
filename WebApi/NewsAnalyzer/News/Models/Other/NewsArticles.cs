using Amazon.DynamoDBv2.DataModel;
using News.Models.Request;

namespace News.Models.Other
{
    [DynamoDBTable("news-articles")]
    public class NewsArticles : OperationalStaus
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; } = String.Empty;
        [DynamoDBIgnore]
        public string Code { get; set; } = string.Empty;
        [DynamoDBProperty("articles")]
        public List<Article> Articles { get; set; } = new List<Article>();
        [DynamoDBProperty("originatingRequest")]
        public NewsArticleRequest OriginatingRequest { get; set; } = new NewsArticleRequest();
    }
}