﻿using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Static;

namespace NewsAnalyzer.Services
{
    public class ControllerResponseService
    {
        public ObjectResult ErrorResponse(ErrorStatus errStatus, string message)
        {
            const string unknownMsg = "An unknown error occured";
            message = message == null || errStatus == ErrorStatus.NoError ? unknownMsg : message;
            errStatus = errStatus == ErrorStatus.NoError ? ErrorStatus.InternalServerError : errStatus;
            return new ObjectResult(message) { StatusCode = (int)errStatus };
        }
    }
}
