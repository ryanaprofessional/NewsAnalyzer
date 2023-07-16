using Ai.Models.Other.Ai;
using Ai.Clients;
using System.Text.Json;
using Ai.Static;
using System.Net;

namespace Ai.Repositories
{
    public class OpenAiRepository
    {
        private readonly OpenAiClient _openAiClient;
        public OpenAiRepository(OpenAiClient openAiClient)
        {
            _openAiClient = openAiClient;
        }

        public async Task<ChatAnswer> SummarizeNewsArticles(StringContent postRequestContent)
        {
            ChatAnswer summaryOfArticles = new ChatAnswer();
            // A logging system will be implemented in later commits.
            const string internalErrorDefaultMessage = "An internal error occured.  We are aware of this issue and are addressing.  Your specific error message is: ";

            try
            {
                HttpResponseMessage completionsResponse = await _openAiClient.GetCompletions(postRequestContent);
                summaryOfArticles.IsSuccess = completionsResponse.IsSuccessStatusCode;
                
                if(summaryOfArticles.IsSuccess)
                {
                    var responseContent = await completionsResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var x = JsonSerializer.Deserialize<ChatChoices>(responseContent, options);
                    summaryOfArticles.Answer = JsonSerializer.Deserialize<ChatChoices>(responseContent, options).Choices[0].Message.Content;
                    summaryOfArticles.IsSuccess = true;
                }
                else
                {
                    var responseContent = await completionsResponse.Content.ReadAsStringAsync();
                    var document = JsonDocument.Parse(responseContent);
                    var errMessage = document.RootElement.GetProperty("error").GetProperty("message").GetString();
                    summaryOfArticles.ErrorStatus = GetErrorStatus(completionsResponse.StatusCode);
                    summaryOfArticles.Message = GetErrorMessage(completionsResponse.StatusCode, errMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                summaryOfArticles.Code = ErrorCode.Custom;
                summaryOfArticles.ErrorStatus = ErrorStatus.BadRequest;
                summaryOfArticles.Message = internalErrorDefaultMessage + "Failed to complete 3rd party HTTPRequest.  This error can be thrown if there is an issue with the HTTP request, such as network connectivity problems, DNS resolution failures, or server-side error.";
            }
            catch (TaskCanceledException ex)
            {
                summaryOfArticles.Code = ErrorCode.Custom;
                summaryOfArticles.ErrorStatus = ErrorStatus.Timeout;
                summaryOfArticles.Message = internalErrorDefaultMessage + "3rd party HTTP request timed out or was canceled before receiving a response.";
            }
            catch (JsonException ex)
            {
                summaryOfArticles.Code = ErrorCode.Custom;
                summaryOfArticles.ErrorStatus = ErrorStatus.UnprocessableEntity;
                summaryOfArticles.Message = internalErrorDefaultMessage + "Possible malformed JSON or incompatible data types when parsing response from 3rd party.";
            }
            catch (InvalidOperationException ex)
            {
                summaryOfArticles.Code = ErrorCode.Custom;
                summaryOfArticles.ErrorStatus = ErrorStatus.NotFound;
                summaryOfArticles.Message = internalErrorDefaultMessage + "Trouble making valid 3rd party HTTPRequest to retrieve news articles.  Potentially bad URL.";
            }
            catch (Exception ex)
            {
                summaryOfArticles.Code = ErrorCode.Custom;
                summaryOfArticles.ErrorStatus = ErrorStatus.InternalServerError;
                summaryOfArticles.Message = internalErrorDefaultMessage + "Novel error causing breaking problem resulting in failure to retrieve data from 3rd party api.";
            }

            return summaryOfArticles;
        }

        /// <summary>
        /// Returns the appropriate errorstatus from the 3rd party openAi api
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private ErrorStatus GetErrorStatus(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return ErrorStatus.InternalServerError;
                case HttpStatusCode.TooManyRequests:
                    return ErrorStatus.TooManyRequests;
                case HttpStatusCode.ServiceUnavailable:
                    return ErrorStatus.ServiceUnavailable;
                default:
                    return ErrorStatus.InternalServerError;
            }
        }

        /// <summary>
        /// Returns the appropriate error message from the 3rd party openAi api.  Designed to prevent giving too much information away about internal processes.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private string GetErrorMessage(HttpStatusCode statusCode, string errorMessage)
        {
            // In future commits we will have a logging system in place.
            const string apiMessage = "Issue with 3rd party api.  We are aware of the issue and are addressing. The error is related to ";
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return apiMessage + " unauthorization.  This is not related to your level of authorization with our services.";
                case HttpStatusCode.TooManyRequests:
                    return apiMessage + "too many requests on the 3rd party api either from you or on that service in genera.  Please wait some time and try again later.";
                case HttpStatusCode.ServiceUnavailable:
                    return apiMessage + "the service being temporariliy down.  Please wait some time and try again later.";
                default:
                    return apiMessage + "an unknown source at this time.";
            }
        }
    }
}
