using System.Text.Json.Serialization;

namespace Ai.Models.Other.Ai
{
    public class MessageContent
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
