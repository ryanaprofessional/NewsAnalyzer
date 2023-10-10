using News.Models.Other;
using News.Static;
using System.Text.Json;

namespace News.Clients
{
    /// <summary>
    /// Makes requests to newsapi.org
    /// </summary>
    public class NewsApiClient
    {
        private HttpClient _httpClient = new HttpClient();
        private readonly string baseUrl = "https://newsapi.org/v2/";
        private readonly ILogger<NewsApiClient> _logger;

        public NewsApiClient(ILogger<NewsApiClient> logger)
        {
            _httpClient.DefaultRequestHeaders.Add("user-agent", "NewsAPI.Net");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", Environment.GetEnvironmentVariable("NewsApiKey"));
            _logger = logger;
        }

        /// <summary>
        /// Retrieves news articles from the 3rd party news api
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<NewsArticles> GetNewsArticles(string query)
        {
            NewsArticles newsArticles = new NewsArticles();
            // A logging system will be implemented in later commits.
            const string internalErrorDefaultMessage = "An internal error occured.  We are aware of this issue and are addressing.  Your specific error message is: ";
            try
            {
                var newsArticlesResponse = await _httpClient.GetAsync(baseUrl + "everything?" + query);
                var responseContent = await newsArticlesResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                newsArticles = JsonSerializer.Deserialize<NewsArticles>(responseContent, options);
                newsArticles.IsSuccess = newsArticlesResponse.IsSuccessStatusCode;
                if (!newsArticles.IsSuccess)
                {
                    _logger.LogError("Unable to retrieve from news api");
                    _logger.LogError($"Error Code: {newsArticles.Code} --- Error Message: {newsArticles.Message}");
                    newsArticles.ErrorStatus = GetErrorStatus(newsArticles.Code);
                    newsArticles.Message = GetErrorMessage(newsArticles.Code, newsArticles.Message);
                }
            }
            catch (HttpRequestException)
            {
                newsArticles.ErrorStatus = ErrorStatus.BadRequest;
                newsArticles.Message = internalErrorDefaultMessage + "Failed to complete 3rd party HTTPRequest.  This error can be thrown if there is an issue with the HTTP request, such as network connectivity problems, DNS resolution failures, or server-side error.";
            }
            catch (TaskCanceledException)
            {
                newsArticles.ErrorStatus = ErrorStatus.Timeout;
                newsArticles.Message = internalErrorDefaultMessage + "3rd party HTTP request timed out or was canceled before receiving a response.";
            }
            catch (JsonException)
            {
                newsArticles.ErrorStatus = ErrorStatus.UnprocessableEntity;
                newsArticles.Message = internalErrorDefaultMessage + "Possible malformed JSON or incompatible data types when parsing response from 3rd party.";
            }
            catch (InvalidOperationException)
            {
                newsArticles.ErrorStatus = ErrorStatus.NotFound;
                newsArticles.Message = internalErrorDefaultMessage + "Trouble making valid 3rd party HTTPRequest to retrieve news articles.  Potentially bad URL.";
            }
            catch (Exception)
            {
                newsArticles.ErrorStatus = ErrorStatus.InternalServerError;
                newsArticles.Message = internalErrorDefaultMessage + "Novel error causing breaking problem resulting in failure to retrieve data from 3rd party api.";
            }

            return newsArticles;
        }

        /// <summary>
        /// Returns the appropriate errorstatus from the 3rd party news api
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private ErrorStatus GetErrorStatus(string errorCode)
        {
            switch (errorCode)
            {
                case "apiKeyDisabled":
                    return ErrorStatus.InternalServerError;
                case "apiKeyExhausted":
                    return ErrorStatus.InternalServerError;
                case "apiKeyInvalid":
                    return ErrorStatus.InternalServerError;
                case "apiKeyMissing":
                    return ErrorStatus.InternalServerError;
                case "parameterInvalid":
                    return ErrorStatus.UnprocessableEntity;
                case "parametersMissing":
                    return ErrorStatus.BadRequest;
                case "rateLimited":
                    return ErrorStatus.TooManyRequests;
                case "sourcesTooMany":
                    return ErrorStatus.BadRequest;
                case "sourceDoesNotExist":
                    return ErrorStatus.BadRequest;
                case "unexpectedError":
                    return ErrorStatus.InternalServerError;
                default:
                    return ErrorStatus.InternalServerError;
            }
        }

        /// <summary>
        /// Returns the appropriate error message from the 3rd party news api.  Designed to prevent giving too much information away about internal processes.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private string GetErrorMessage(string errorCode, string errorMessage)
        {
            const string authmessage = "authorization with 3rd party api service..";
            const string apiMessage = "Issue with 3rd party api.  We are aware of the issue and are addressing.  The issue relates to ";
            switch (errorCode)
            {
                case "apiKeyDisabled":
                    return apiMessage + authmessage;
                case "apiKeyExhausted":
                    return apiMessage + authmessage;
                case "apiKeyInvalid":
                    return apiMessage + authmessage;
                case "apiKeyMissing":
                    return apiMessage + authmessage;
                case "parameterInvalid":
                    return apiMessage + "the 3rd party service updating their models before we had a chance to get to it.";
                case "parametersMissing":
                    return "Required parameters are missing from the request and it cannot be completed";
                case "rateLimited":
                    return "You have been rate limited. Back off for a while before trying the request again";
                case "sourcesTooMany":
                    return "You have requested too many sources in a single request. Try splitting the request into 2 smaller requests.";
                case "sourceDoesNotExist":
                    return "You have requested a source which does not exist";
                default:
                    return apiMessage + " an unknown/unexpected error";
            }
        }
    }
}
