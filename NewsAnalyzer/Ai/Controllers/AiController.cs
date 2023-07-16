using Microsoft.AspNetCore.Mvc;
using Ai.Models.Other;
using Ai.Models.Other.News;
using Ai.Services;
using Ai.Models.Other.Ai;

namespace Ai.Controllers
{
    public class AiController : Controller
    {
        private PersistenceService _persistenceService;
        private AiQueryService _aiQueryService;
        private ControllerResponseService _controllerResponseService;

        public AiController(PersistenceService persistenceService, AiQueryService aiQueryService, ControllerResponseService controllerResponseService)
        {
            _persistenceService = persistenceService;
            _aiQueryService = aiQueryService;
            _controllerResponseService = controllerResponseService;
        }

        [HttpGet("/ai/news/{id}", Name = "SummarizeArticles")]
        public async Task<IActionResult> SummarizeArticles(int id)
        {
            PersistedResult persistRetrieval = await _persistenceService.GetNewsArticles(id);
            NewsArticles newsArticles = (NewsArticles)persistRetrieval.Result;
            ChatAnswer summary = await _aiQueryService.ReduceAndSummarizeArticles(newsArticles);
            if (summary.IsSuccess)
            {
                return Ok(summary.Answer);
            }
            else
            {
                return _controllerResponseService.ErrorResponse(summary.ErrorStatus, summary.Message);
            }
        }
    }
}
