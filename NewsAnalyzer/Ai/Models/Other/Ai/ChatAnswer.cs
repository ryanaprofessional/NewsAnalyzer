using System.Text.Json.Serialization;
using Ai.Static;

namespace Ai.Models.Other.Ai
{
    public class ChatAnswer
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; } = ErrorCode.NoError;

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public ErrorStatus ErrorStatus { get; set; } = ErrorStatus.NoError;

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
    }
}
