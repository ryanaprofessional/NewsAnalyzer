using Ai.Static;

namespace Ai.Models.Other
{
    public class PersistedResult
    {
        public int Id { get; set; } = 1;
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public ErrorStatus ErrorStatus { get; set; } = ErrorStatus.NoError;
        public object Result { get; set; } = 0;
    }
}
