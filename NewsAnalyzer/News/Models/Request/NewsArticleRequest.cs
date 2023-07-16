using System.ComponentModel.DataAnnotations;

namespace News.Models.Request
{
    public class NewsArticleRequest
    {
        [Required]
        public string Q { get; set; } = string.Empty;
        public string SearchIn { get; set; } = string.Empty;
        public string Sources { get; set; } = string.Empty;
        public string Domains { get; set; } = string.Empty;
        public string ExcludeDomains { get; set; } = string.Empty;
        public DateTime? From { get; set; } = null;
        public DateTime? To { get; set; } = null;
        public string Language { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
        public int? PageSize { get; set; } = null;
        public int? Page { get; set; } = null;
    }

}
