using System.Text.Json;
using Ai.Models.Other;
using Ai.Models.Other.News;
using Ai.Static;

namespace Ai.Repositories
{
    /// <summary>
    /// Layer for persisting and retrieving persisted news articles.
    /// </summary>
    public class ArticleRepository
    {
        private readonly string localPathForNewsArticles;
        private readonly IConfiguration _config;
        public ArticleRepository(IConfiguration config)
        {
            _config = config;
            localPathForNewsArticles = _config.GetValue<string>("JsonFilePath");
        }

        public async Task<PersistedResult> GetNewsArticles(int id)
        {
            return ReadFromJsonFile<NewsArticles>(localPathForNewsArticles);
        }

        private PersistedResult ReadFromJsonFile<T>(string filePath)
        {
            PersistedResult retrievalResult = new PersistedResult();

            try
            {
                string jsonString = File.ReadAllText(filePath);
                retrievalResult.Result = JsonSerializer.Deserialize<T>(jsonString);
                retrievalResult.IsSuccess = true;
                return retrievalResult;
            }
            catch (JsonException ex)
            {
                retrievalResult.Code = ErrorCode.Custom;
                retrievalResult.ErrorStatus = ErrorStatus.InternalServerError;
                retrievalResult.Message = "Error occurred during JSON serialization";
            }
            catch (IOException ex)
            {
                retrievalResult.Code = ErrorCode.Custom;
                retrievalResult.ErrorStatus = ErrorStatus.InternalServerError;
                retrievalResult.Message = "IO error occurred while writing JSON data";
            }
            catch (UnauthorizedAccessException ex)
            {
                retrievalResult.Code = ErrorCode.Custom;
                retrievalResult.ErrorStatus = ErrorStatus.InternalServerError;
                retrievalResult.Message = "Access denied while writing JSON data";
            }
            catch (ArgumentException ex)
            {
                retrievalResult.Code = ErrorCode.Custom;
                retrievalResult.ErrorStatus = ErrorStatus.InternalServerError;
                retrievalResult.Message = "Invalid file path";
            }
            catch (Exception ex)
            {
                retrievalResult.Code = ErrorCode.Custom;
                retrievalResult.ErrorStatus = ErrorStatus.InternalServerError;
                retrievalResult.Message = "Unhandled error occurred";
            }

            return retrievalResult;
        }
    }
}