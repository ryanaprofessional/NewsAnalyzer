using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Models.Request;
using NewsAnalyzer.Models.Other;
using NewsAnalyzer.Services;

namespace NewsAnalyzer.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly QueryNewsApiService _apiQueryService;
        private readonly ControllerResponseService _controllerResponseService;
        private readonly PersistNewsArticleService _persistNewsArticleService;

        public NewsController(ILogger<NewsController> logger, QueryNewsApiService apiQueryService, ControllerResponseService controllerResponseService, PersistNewsArticleService persistNewsArticleService)
        {
            _logger = logger;
            _apiQueryService = apiQueryService;
            _controllerResponseService = controllerResponseService;
            _persistNewsArticleService = persistNewsArticleService;
        }

        /// <summary>
        /// Post endpoint for searching for news articles that meet specific criteria, then persisting the results.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("/news/persist", Name = "PersistNewsArticles")]
        public async Task<IActionResult> PersistNewsArticles([FromBody] NewsArticleRequest parameters)
        {
            NewsArticles newsArticles = await _apiQueryService.QueryNewsApi(parameters);
            if (!newsArticles.IsSuccess)
            {
                return _controllerResponseService.ErrorResponse(newsArticles.ErrorStatus, newsArticles.Message);
            }

            PersistedResult persistedResult = await _persistNewsArticleService.PersistNewsArticles(newsArticles);
            if (persistedResult.IsSuccess)
            {
                return Ok(persistedResult);
            }
            else
            {
                return _controllerResponseService.ErrorResponse(persistedResult.ErrorStatus, persistedResult.Message);
            }
        }
    }
}

