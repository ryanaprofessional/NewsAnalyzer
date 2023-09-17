using Amazon.DynamoDBv2.DataModel;
using News.Models.Other;
using News.Static;

namespace News.Repositories
{
    /// <summary>
    /// Layer for persisting and retrieving persisted news articles.
    /// </summary>
    public class ArticleRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<ArticleRepository> _logger;

        public ArticleRepository(IConfiguration config, IDynamoDBContext dynamoDbContext, ILogger<ArticleRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        public async Task<PersistedResult> PersistNewsArticles(NewsArticles newsArticles)
        {
            var persisted = new PersistedResult();
            try
            {
                var newId =  await GetUniqueGuidForNewsArticle();
                if(newId == null)
                {
                    persisted.ErrorStatus = ErrorStatus.InternalServerError;
                    persisted.Message = "Unable to assign key, item not persisted.";
                }
                else
                {
                    newsArticles.Id = newId.ToString();
                    await _dynamoDbContext.SaveAsync(newsArticles);
                    persisted.IsSuccess = true;
                }
                persisted.Id = newId;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                persisted.Id = null;
                persisted.ErrorStatus = ErrorStatus.InternalServerError;
                persisted.Message = "Unable to persist.";
                persisted.IsSuccess = false;
            }

            return persisted;
        }

        /// <summary>
        /// Generates a new guid for the news articles table
        /// </summary>
        private async Task<string?> GetUniqueGuidForNewsArticle()
        {

            string? newId = null;
            try
            {
                var StopTime = System.DateTime.Now.AddMinutes(2);

                while(System.DateTime.Now < StopTime)
                {
                    string tempId = Guid.NewGuid().ToString();
                    var newsArticle = await _dynamoDbContext.LoadAsync<NewsArticles>(tempId);
                    if (newsArticle == null)
                    {
                        newId = tempId;
                        break;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return newId;
        }
    }
}