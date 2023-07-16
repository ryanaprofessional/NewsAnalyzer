using System.ComponentModel.DataAnnotations;

namespace News.Models.Request
{
    public class NewsArticleRequest
    {
        [Required]
        public string Q { get; set; } = string.Empty;
    }

}
