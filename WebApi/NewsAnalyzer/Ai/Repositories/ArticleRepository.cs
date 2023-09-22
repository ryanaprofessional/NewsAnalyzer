using Ai.Models.Other.News;
using Ai.Static;
using Amazon.DynamoDBv2.DataModel;

namespace Ai.Repositories
{
    /// <summary>
    /// Layer for persisting and retrieving persisted news articles.
    /// </summary>
    public class ArticleRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger _logger;

        public ArticleRepository(IDynamoDBContext dynamoDbContext, ILogger<ArticleRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        public async Task<NewsArticles> GetNewsArticlesByid(string id)
        {
            var articles = new NewsArticles();
            try
            {
                articles = await _dynamoDbContext.LoadAsync<NewsArticles>(id);
                if (articles == null)
                {
                    throw new FileNotFoundException();
                }
                articles.IsSuccess = true;
            }
            catch (FileNotFoundException)
            {
                _logger.LogError("DynamoDb could not find the id: " + id);
                articles.ErrorStatus = ErrorStatus.NotFound;
                articles.Message = "No items found with id of: " + id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                articles.ErrorStatus = ErrorStatus.InternalServerError;
                articles.Message = "Unhandled error occurred";
            }

            return articles;
        }
    }
}