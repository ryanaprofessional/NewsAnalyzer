using System.ComponentModel.DataAnnotations;

namespace Ai.Models.Other.News
{
    public class NewsArticleRequest
    {
        [Required]
        public string Q { get; set; } = string.Empty;
    }

}
