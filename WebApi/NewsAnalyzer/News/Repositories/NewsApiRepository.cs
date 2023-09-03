using News.Clients;
using System.Text.Json;
using News.Models.Other;
using News.Static;

namespace News.Repositories
{

    /// <summary>
    /// Repository is responsible for interacting with the newsApiClient
    /// </summary>
    public class NewsApiRepository
    {
        private NewsApiClient _client;
        public NewsApiRepository(NewsApiClient newsApiClient)
        {
            _client = newsApiClient;
        }

        /// <summary>
        /// Get News Articles from 3rd party api and return an object containing the results (if successful), and messages about the job.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<NewsArticles> GetNewsArticles(string query)
        {         
            return await _client.GetNewsArticles(query);
        }
    }
}
