using Ai.Repositories;
using System.Text.Json;
using System.Text;
using Ai.Models.Other.Ai;
using Ai.Models.Other.News;
using Ai.Static;

namespace Ai.Services
{

    /// <summary>
    /// This Service is responsible for creating queries from request objects, and using those queries to feed to 
    /// the repositories, and then return the responses from the repositories.  
    /// </summary>
    public class AiQueryService
    {
        private readonly OpenAiRepository _openAiRepository;
        public AiQueryService(OpenAiRepository openAiRepository) 
        {
            _openAiRepository = openAiRepository;
        }

        /// <summary>
        /// Receives NewsApi Articles and then converts them into just the titles and descriptions, and then passes them to the openAi Repository.
        /// </summary>
        /// <param name="newsArticlesToSummarize"></param>
        /// <returns></returns>
        public async Task<ChatAnswer> ReduceAndSummarizeArticles(NewsArticles newsArticlesToSummarize)
        {

            try
            {
                MessageContent[] messages = new MessageContent[]
{
                new MessageContent { Role = "system", Content = "I will send you a series of news articles and you are to abbreviate them and give me a short summary of what is going on in the form of a paragraph.  Do not include the titles, this is supposed to be a collective summary." },
                new MessageContent { Role = "user", Content = ReduceNewsArticles(newsArticlesToSummarize)}
};

                ChatCompletions request = new ChatCompletions
                {
                    Model = "gpt-3.5-turbo",
                    Messages = messages
                };

                string jsonPayload = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var res = await _openAiRepository.SummarizeNewsArticles(content);
                return res;
            }
            catch
            {
                return new ChatAnswer
                {
                    Code = "custom",
                    ErrorStatus = ErrorStatus.InternalServerError,
                    Message = "Internal Error.  Failed to reduce the news articles to a summarizable format."
                };
            }
        }

        private string ReduceNewsArticles(NewsArticles newsArticles)
        {
            // This is a temporary solution until we implement paging.
            string concatenatedOutput = string.Empty;
            foreach(Article article in newsArticles.Articles)
            {
                concatenatedOutput += " --- Title:" + article.Title + " Desc: " + article.Description;
            }
            return ShortenString(concatenatedOutput);
        }

        public string ShortenString(string text)
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
    }
}