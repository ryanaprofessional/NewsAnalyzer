using System.Text.Json.Serialization;
using Ai.Static;

namespace Ai.Models.Other.Ai
{
    public class ChatAnswer : OperationalStaus
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
    }
}
