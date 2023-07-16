using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Ai.Clients
{
    public class OpenAiClient
    {
        private HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _config;
        private readonly string baseUrl = "https://api.openai.com/v1/";

        public OpenAiClient(IConfiguration config)
        {
            _config = config;
            string apiKey = _config.GetValue<string>("OpenAiKey");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<HttpResponseMessage> GetCompletions(StringContent postRequestContent)
        {
            return await _httpClient.PostAsync(baseUrl + "/chat/completions", postRequestContent);
            
        }
    }
}