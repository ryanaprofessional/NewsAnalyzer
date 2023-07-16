namespace News.Clients
{
    /// <summary>
    /// Makes requests to newsapi.org
    /// </summary>
    public class NewsApiClient
    {
        private HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _config;
        private readonly string baseUrl = "https://newsapi.org/v2/";
        public NewsApiClient(IConfiguration config)
        {
            _config = config;
            _httpClient.DefaultRequestHeaders.Add("user-agent", "NewsAPI.Net");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _config.GetValue<string>("NewsApiKey"));
        }

        public async Task<HttpResponseMessage> GetNewsArticles(string query)
        {
            return await _httpClient.GetAsync(baseUrl + "everything?" + query);
        }
    }
}
