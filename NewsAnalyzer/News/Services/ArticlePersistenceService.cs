using News.Repositories;
using News.Models.Other;

namespace News.Services
{
    /// <summary>
    /// This service is responsible for communication between the controllers and the persistence layer.
    /// </summary>
    public class ArticlePersistenceService
    {
        private readonly ArticleRepository _newsArticleRepository;
        public ArticlePersistenceService(ArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task<PersistedResult> PersistNewsArticles(NewsArticles newsArticles)
        {
            return await _newsArticleRepository.PersistNewsArticles(newsArticles);
        }
        public async Task<PersistedResult> GetNewsArticles(int id)
        {
            return await _newsArticleRepository.GetNewsArticles(id);
        }
    }
}