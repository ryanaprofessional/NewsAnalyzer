using Ai.Repositories;
using Ai.Models.Other;

namespace Ai.Services
{
    /// <summary>
    /// This service is responsible for communication between the controllers and the persistence layer.
    /// </summary>
    public class PersistenceService
    {
        private readonly ArticleRepository _newsArticleRepository;
        public PersistenceService(ArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task<PersistedResult> GetNewsArticles(int id)
        {
            return await _newsArticleRepository.GetNewsArticles(id);
        }
    }
}