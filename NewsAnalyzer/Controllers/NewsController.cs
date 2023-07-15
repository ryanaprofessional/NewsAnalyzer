using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Models.Request;
using NewsAnalyzer.Models.Other;
using NewsAnalyzer.Services;
using NewsAnalyzer.Static;

namespace NewsAnalyzer.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly QueryNewsApiService _apiQueryService;
        private readonly ControllerResponseService _controllerResponseService;

        public NewsController(ILogger<NewsController> logger, QueryNewsApiService apiQueryService, ControllerResponseService controllerResponseService)
        {
            _logger = logger;
            _apiQueryService = apiQueryService;
            _controllerResponseService = controllerResponseService;
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

            return null;
        }
    }
}

