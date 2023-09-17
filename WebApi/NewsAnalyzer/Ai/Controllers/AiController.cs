using Microsoft.AspNetCore.Mvc;
using Ai.Models.Other.News;
using Ai.Models.Other.Ai;
using Ai.Repositories;
using Ai.Extensions;

namespace Ai.Controllers
{
    public class AiController : Controller
    {
        private OpenAiRepository _openAiRepository;
        private ArticleRepository _articleRepository;
        private readonly IConfiguration _config;

        public AiController(OpenAiRepository openAiRepository, ArticleRepository articleRepository, IConfiguration config)
        {
            _openAiRepository = openAiRepository;
            _articleRepository = articleRepository;
            _config = config;
        }

        [HttpGet("/ai/news/{id}", Name = "SummarizeArticles")]
        public async Task<IActionResult> SummarizeArticles(string id)
        {
            var articles = await _articleRepository.GetNewsArticlesByid(id);
            if (!articles.IsSuccess)
            {
                return articles.ErrorStatus.ToHttpResponse(articles.Message);
            }

            var summary = await GenerateSummary(articles);

            if (summary.IsSuccess)
            {
                return Ok(summary.Answer);
            }
            else
            {
                return summary.ErrorStatus.ToHttpResponse(articles.Message);
            }
        }

        private string ReduceNewsArticles(NewsArticles newsArticles)
        {
            // This is a temporary solution until we implement paging.
            string concatenatedOutput = string.Empty;
            foreach (Article article in newsArticles.Articles)
            {
                concatenatedOutput += " --- Title:" + article.Title + " Desc: " + article.Description;
            }
            return ShortenString(concatenatedOutput);
        }

        private string ShortenString(string text)
        {
            // This is a temporary solution until we implement paging.
            int wordCount = text.Split(' ').Length;
            int charCount = text.Length;
            double tokensCountWordEst = wordCount / 0.75;
            double tokensCountCharEst = charCount / 4.0;

            int output = (int)Math.Max(tokensCountWordEst, tokensCountCharEst);
            int maxLength = Math.Min(output, 4050);
            string shortenedString = text.Substring(0, maxLength);
            return shortenedString;
        }
        private async Task<ChatAnswer> GenerateSummary(NewsArticles articles)
        {
            MessageContent[] messages = new MessageContent[]
            {
                new MessageContent { Role = "system", Content = "I will send you a series of news articles and you are to abbreviate them and give me a short summary of what is going on in the form of a paragraph. Do not include the titles, this is supposed to be a collective summary." },
                new MessageContent { Role = "user", Content = ReduceNewsArticles(articles) }
            };

            ChatCompletions request = new ChatCompletions
            {
                Model = _config.GetValue<string>("ChatModel"),
                Messages = messages
            };

            return await _openAiRepository.SummarizeNewsArticles(request);
        }
    }
}
