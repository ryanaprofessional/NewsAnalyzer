using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Ai.Models.Other.Ai;
using Ai.Static;
using System.Net;

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

        public async Task<ChatAnswer> GetCompletions<T>(ChatCompletions completionRequest)
        {
            // A logging system will be implemented in later commits.
            const string internalErrorDefaultMessage = "An internal error occured.  We are aware of this issue and are addressing.  Your specific error message is: ";

            ChatAnswer completionResponse = new ChatAnswer();
            try
            {
                string jsonPayload = JsonSerializer.Serialize(completionRequest);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage completionsResponse = await _httpClient.PostAsync(baseUrl + "/chat/completions", content);
                completionResponse.IsSuccess = completionsResponse.IsSuccessStatusCode;

                if (completionResponse.IsSuccess)
                {
                    var responseContent = await completionsResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var x = JsonSerializer.Deserialize<ChatChoices>(responseContent, options);
                    completionResponse.Answer = JsonSerializer.Deserialize<ChatChoices>(responseContent, options).Choices[0].Message.Content;
                    completionResponse.IsSuccess = true;
                }
                else
                {
                    var responseContent = await completionsResponse.Content.ReadAsStringAsync();
                    var document = JsonDocument.Parse(responseContent);
                    var errMessage = document.RootElement.GetProperty("error").GetProperty("message").GetString();
                    completionResponse.ErrorStatus = GetErrorStatus(completionsResponse.StatusCode);
                    completionResponse.Message = GetErrorMessage(completionsResponse.StatusCode, errMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                completionResponse.ErrorStatus = ErrorStatus.BadRequest;
                completionResponse.Message = internalErrorDefaultMessage + "Failed to complete 3rd party HTTPRequest.  This error can be thrown if there is an issue with the HTTP request, such as network connectivity problems, DNS resolution failures, or server-side error.";
            }
            catch (TaskCanceledException ex)
            {
                completionResponse.ErrorStatus = ErrorStatus.Timeout;
                completionResponse.Message = internalErrorDefaultMessage + "3rd party HTTP request timed out or was canceled before receiving a response.";
            }
            catch (JsonException ex)
            {
                completionResponse.ErrorStatus = ErrorStatus.UnprocessableEntity;
                completionResponse.Message = internalErrorDefaultMessage + "Possible malformed JSON or incompatible data types when parsing response from 3rd party.";
            }
            catch (InvalidOperationException ex)
            {
                completionResponse.ErrorStatus = ErrorStatus.NotFound;
                completionResponse.Message = internalErrorDefaultMessage + "Trouble making valid 3rd party HTTPRequest to retrieve news articles.  Potentially bad URL.";
            }
            catch (Exception ex)
            {
                completionResponse.ErrorStatus = ErrorStatus.InternalServerError;
                completionResponse.Message = internalErrorDefaultMessage + "Novel error causing breaking problem resulting in failure to retrieve data from 3rd party api.";
            }

            return completionResponse;
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