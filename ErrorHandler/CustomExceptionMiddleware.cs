using ErrorHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ErrorHandler
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly IErrorOperations _errorOperations;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger, IErrorOperations errorOperations)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorOperations = errorOperations ?? throw new ArgumentNullException(nameof(errorOperations));
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (CustomException ex1)
            {
                await HandleExceptionAsync(context, ex1);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as BaseCustomException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var customcode = "1001";
            var message = "Unexpected error";
            var description = "Unexpected error";
            var extraparameter = "";
            var isLogged = false;
            var errorTrackingid = "";

            if (null != customException)
            {
                customcode = customException.CustomCode;
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
                extraparameter = customException.Extraparameter;
                isLogged = customException.isLogged;
                errorTrackingid = customException.ErrorTrackingId;

                string key = customcode;

                if (description == null && _errorOperations != null)
                {
                    description = await _errorOperations.GetErrorDescription(key, extraparameter);
                }
            }

            if (!isLogged)
            {
                if (!String.IsNullOrEmpty(errorTrackingid))
                    _logger.LogError("UserId: {@UserId}, " + customcode + " " + message + " " + description + " " + exception.Message + " " + exception.StackTrace + " ErrorTrackingId {@ErrorTrackingId}", ErrorHelperMethods.GetUserId(context), errorTrackingid);
                else
                    _logger.LogError("UserId: {@UserId}, " + customcode + " " + message + " " + description + " " + exception.Message + " " + exception.StackTrace, ErrorHelperMethods.GetUserId(context));
            }


            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                error = new Error
                {
                    Code = customcode,
                    Message = message,
                    Description = description
                }
            }));
        }


    }
}
