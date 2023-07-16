using News.Clients;
using System.Text.Json;
using News.Models.Other;
using News.News.Static;

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
            NewsArticles newsArticles = new NewsArticles();
            // A logging system will be implemented in later commits.
            const string internalErrorDefaultMessage = "An internal error occured.  We are aware of this issue and are addressing.  Your specific error message is: ";
            try
            {
                var newsArticlesResponse = await _client.GetNewsArticles(query);
                var responseContent = await newsArticlesResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                newsArticles = JsonSerializer.Deserialize<NewsArticles>(responseContent, options);
                newsArticles.IsSuccess = newsArticlesResponse.IsSuccessStatusCode;
                if (!newsArticles.IsSuccess)
                {
                    newsArticles.ErrorStatus = GetErrorStatus(newsArticles.Code);
                    newsArticles.Message = GetErrorMessage(newsArticles.Code, newsArticles.Message);
                }
            }
            catch (HttpRequestException ex)
            {
                newsArticles.Code = ErrorCode.Custom;
                newsArticles.ErrorStatus = ErrorStatus.BadRequest;
                newsArticles.Message = internalErrorDefaultMessage + "Failed to complete 3rd party HTTPRequest.  This error can be thrown if there is an issue with the HTTP request, such as network connectivity problems, DNS resolution failures, or server-side error.";
            }
            catch (TaskCanceledException ex)
            {
                newsArticles.Code = ErrorCode.Custom;
                newsArticles.ErrorStatus = ErrorStatus.Timeout;
                newsArticles.Message = internalErrorDefaultMessage + "3rd party HTTP request timed out or was canceled before receiving a response.";
            }
            catch (JsonException ex)
            {
                newsArticles.Code = ErrorCode.Custom;
                newsArticles.ErrorStatus = ErrorStatus.UnprocessableEntity;
                newsArticles.Message = internalErrorDefaultMessage + "Possible malformed JSON or incompatible data types when parsing response from 3rd party.";
            }
            catch (InvalidOperationException ex)
            {
                newsArticles.Code = ErrorCode.Custom;
                newsArticles.ErrorStatus = ErrorStatus.NotFound;
                newsArticles.Message = internalErrorDefaultMessage + "Trouble making valid 3rd party HTTPRequest to retrieve news articles.  Potentially bad URL.";
            }
            catch (Exception ex)
            {
                newsArticles.Code = ErrorCode.Custom;
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
            const string apiMessage = "Issue with 3rd party api.  We are aware of the issue and are addressing";
            switch (errorCode)
            {
                case "apiKeyDisabled":
                    return apiMessage;
                case "apiKeyExhausted":
                    return apiMessage;
                case "apiKeyInvalid":
                    return apiMessage;
                case "apiKeyMissing":
                    return apiMessage;
                default:
                    return "Error querying 3rd party api.  Error message is: " + errorMessage;
            }
        }
    }
}
