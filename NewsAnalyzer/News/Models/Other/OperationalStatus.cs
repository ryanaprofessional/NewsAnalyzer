using News.Static;

namespace News.Models.Other
{
    public class OperationalStaus
    {
        public ErrorStatus ErrorStatus { get; set; } = ErrorStatus.NoError;
        public string Message { get; set; } = "No Error Message";
        public bool IsSuccess { get; set; } = false;
    }
}
