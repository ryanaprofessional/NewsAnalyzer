using NewsAnalyzer.Repositories;
using NewsAnalyzer.Models.Request;
using NewsAnalyzer.Static;
using NewsAnalyzer.Models.Other;

namespace NewsAnalyzer.Services
{
    /// <summary>
    /// This Service is responsible for creating queries for the news api
    /// </summary>
    public class QueryNewsApiService
    {
        private readonly NewsApiRepository _newsApiRepository;

        public QueryNewsApiService(NewsApiRepository newsApiRepository) 
        {
            _newsApiRepository = newsApiRepository;
        }

        public async Task<NewsArticles> QueryNewsApi(NewsArticleRequest newsArticleRequest)
        {
            string query;
            try
            {
                newsArticleRequest.Q = SurroundWithQuotes(newsArticleRequest.Q);
                query = ToQueryString(newsArticleRequest);
            } catch
            {
                return new NewsArticles
                {
                    ErrorStatus = ErrorStatus.InternalServerError,
                    Message = "Failed to create query string for news articles."
                };
            }

            return await _newsApiRepository.GetNewsArticles(query);
        }

        private string ToQueryString(object obj)
        {
            var properties = obj.GetType().GetProperties();
            var queryParameters = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if(IsValidQueryParam(value))
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