using Amazon.DynamoDBv2.DataModel;

namespace Ai.Models.Other.News
{
    public class ArticleSource
    {
        [DynamoDBProperty("id")]
        public string Id { get; set; }
        [DynamoDBProperty("name")]
        public string Name { get; set; }
    }
}