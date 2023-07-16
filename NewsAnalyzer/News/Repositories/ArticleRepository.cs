using System.Text.Json;
using News.Models.Other;
using News.Static;

namespace News.Repositories
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

        public async Task<PersistedResult> PersistNewsArticles(NewsArticles newsArticles)
        {
            return WriteToJsonFile(newsArticles, localPathForNewsArticles);
        }
        public async Task<PersistedResult> GetNewsArticles(int id)
        {
            return ReadFromJsonFile<NewsArticles>(localPathForNewsArticles);
        }

        private PersistedResult WriteToJsonFile(object obj, string filePath)
        {
            PersistedResult writeResult = new PersistedResult();

            try
            {
                string jsonString = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(filePath, jsonString);
                writeResult.IsSuccess = true;
                writeResult.Message = "Success";
                return writeResult;
            }
            catch (JsonException ex)
            {
                writeResult.Code = ErrorCode.Custom;
                writeResult.ErrorStatus = ErrorStatus.InternalServerError;
                writeResult.Message = "Error occurred during JSON serialization";
            }
            catch (IOException ex)
            {
                writeResult.Code = ErrorCode.Custom;
                writeResult.ErrorStatus = ErrorStatus.InternalServerError;
                writeResult.Message = "IO error occurred while writing JSON data";
            }
            catch (UnauthorizedAccessException ex)
            {
                writeResult.Code = ErrorCode.Custom;
                writeResult.ErrorStatus = ErrorStatus.InternalServerError;
                writeResult.Message = "Access denied while writing JSON data";
            }
            catch (ArgumentException ex)
            {
                writeResult.Code = ErrorCode.Custom;
                writeResult.ErrorStatus = ErrorStatus.InternalServerError;
                writeResult.Message = "Invalid file path";
            }
            catch (Exception ex)
            {
                writeResult.Code = ErrorCode.Custom;
                writeResult.ErrorStatus = ErrorStatus.InternalServerError;
                writeResult.Message = "Unhandled error occurred";
            }

            return writeResult;
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