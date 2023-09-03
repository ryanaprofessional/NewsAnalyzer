using Microsoft.AspNetCore.Mvc;
using Ai.Static;

namespace Ai.Extensions
{
    public static class ErrorStatusExtensions
    {
        public static ObjectResult ToHttpResponse(this ErrorStatus errStatus, string message)
        {
            const string unknownMsg = "An unknown error occured";
            message = message == null || errStatus == ErrorStatus.NoError ? unknownMsg : message;
            errStatus = errStatus == ErrorStatus.NoError ? ErrorStatus.InternalServerError : errStatus;
            return new ObjectResult(message) { StatusCode = (int)errStatus };
        }
    }
}