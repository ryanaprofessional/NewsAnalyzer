using Amazon.DynamoDBv2.DataModel;

namespace News.Models.Other
{
    public class ArticleSource
    {
        [DynamoDBProperty("id")]
        public string Id { get; set; } = string.Empty;
        [DynamoDBProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}