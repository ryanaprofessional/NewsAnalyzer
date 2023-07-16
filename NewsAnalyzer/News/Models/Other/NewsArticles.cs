using News.News.Static;

namespace News.Models.Other
{
    public class NewsArticles
    {
        public int Id { get; set; } = -1;
        public ErrorStatus ErrorStatus { get; set; } = ErrorStatus.NoError;
        public string Code { get; set; } = ErrorCode.NoError;
        public string Message { get; set; } = "No Error Message";
        public int TotalResults { get; set; } = -1;
        public bool IsSuccess { get; set; } = false;
        public Article[] Articles { get; set; }
    }
}