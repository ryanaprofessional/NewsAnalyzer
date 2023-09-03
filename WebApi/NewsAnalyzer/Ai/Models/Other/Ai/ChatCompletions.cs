using System.Text.Json.Serialization;

namespace Ai.Models.Other.Ai
{
    public class ChatCompletions
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "gpt-3.5-turbo";
        [JsonPropertyName("messages")]
        public MessageContent[] Messages { get; set; }
    }
}
