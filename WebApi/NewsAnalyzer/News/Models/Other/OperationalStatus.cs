using News.Static;
using Amazon.DynamoDBv2.DataModel;

namespace News.Models.Other
{
    public class OperationalStaus
    {
        [DynamoDBIgnore]
        public ErrorStatus ErrorStatus { get; set; } = ErrorStatus.NoError;
        [DynamoDBIgnore]
        public string Message { get; set; } = string.Empty;
        [DynamoDBIgnore]
        public bool IsSuccess { get; set; } = false;
    }
}
