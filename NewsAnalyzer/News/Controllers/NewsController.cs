using Microsoft.AspNetCore.Mvc;
using News.Models.Request;
using News.Models.Other;
using News.Services;
using News.Repositories;
using News.Static;

namespace NewsController
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly NewsApiRepository _newsApiRepository;
        private readonly ControllerResponseService _controllerResponseService;
        private readonly ArticleRepository _articleRepository;

        public NewsController(ILogger<NewsController> logger, NewsApiRepository newsApiRepository, ControllerResponseService controllerResponseService, ArticleRepository articleRepository)
        {
            _logger = logger;
            _newsApiRepository = newsApiRepository;
            _controllerResponseService = controllerResponseService;
            _articleRepository = articleRepository;
        }

        /// <summary>
        /// Post endpoint for searching for news articles that meet specific criteria, then persisting the results.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("/news/persist", Name = "PersistNewsArticles")]
        public async Task<IActionResult> PersistNewsArticles([FromBody] NewsArticleRequest parameters)
        {
            string query;

            try
            {
                query = BuildNewsApiQuery(parameters);
            }
            catch
            {
                return _controllerResponseService.ErrorResponse(ErrorStatus.InternalServerError, "Failed to create query string for news articles.");
            }

            NewsArticles newsArticles = await _newsApiRepository.GetNewsArticles(query);
            if (!newsArticles.IsSuccess)
            {
                return _controllerResponseService.ErrorResponse(newsArticles.ErrorStatus, newsArticles.Message);
            }

            PersistedResult persistedResult = await _articleRepository.PersistNewsArticles(newsArticles);
            if (persistedResult.IsSuccess)
            {
                return Ok(persistedResult.Id);
            }
            else
            {
                return _controllerResponseService.ErrorResponse(persistedResult.ErrorStatus, persistedResult.Message);
            }
        }

        private string BuildNewsApiQuery(NewsArticleRequest newsArticleRequest)
        {
            newsArticleRequest.Q = SurroundWithQuotes(newsArticleRequest.Q);
            return ToQueryString(newsArticleRequest);
        }

        private string ToQueryString(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var queryParameters = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (IsValidQueryParam(value))
                {
                    var parameter = $"{property.Name}={value.ToString()}";
                    queryParameters.Add(parameter);
                }
            }

            return string.Join("&", queryParameters);
        }

        private bool IsValidQueryParam<T>(T val)
        {
            if (val is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                return false;
            }
            else if (val == null)
            {
                return false;
            }

            return true;
        }
        private string SurroundWithQuotes(string input)
        {
            string withoutQuotes = input.Replace("\"", string.Empty);
            string surroundedWithQuotes = $"\"{withoutQuotes}\"";

            return surroundedWithQuotes;
        }
    }
}

