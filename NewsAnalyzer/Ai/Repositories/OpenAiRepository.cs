using Ai.Models.Other.Ai;
using Ai.Models.Other.News;
using Ai.Clients;

namespace Ai.Repositories
{
    public class OpenAiRepository
    {
        private readonly OpenAiClient _openAiClient;
        public OpenAiRepository(OpenAiClient openAiClient)
        {
            _openAiClient = openAiClient;
        }

        public async Task<ChatAnswer> SummarizeNewsArticles(ChatCompletions request)
        {
            return await _openAiClient.GetCompletions<NewsArticles>(request);
        }
    }
}