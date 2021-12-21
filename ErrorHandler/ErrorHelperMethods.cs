using ErrorHandler.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrorHandler
{
    public static class ErrorHelperMethods
    {
        public static List<Error> GenerateFluentValidationErrors(List<string> errorlist, ILoggerFactory LoggerFactory)
        {
            var _logger = LoggerFactory.CreateLogger("validation");
            List<Error> errors = new List<Error>();

            foreach (var item in errorlist)
            {
                try
                {
                    string[] words = item.Split('_'); //uses fluentvalidation error message to get error code and error text
                    Error error = new Error();
                    error.Code = words[0];
                    error.Message = "Bad Request";
                    error.Description = words[1];

                    errors.Add(error);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("1001" + " " + "error code not specified: GenerateValidationErrors method" + " " + ex.Message);

                    Error error = new Error();
                    error.Code = "4002";
                    error.Message = "Bad Request";
                    error.Description = item;

                    errors.Add(error);
                }

                _logger.LogWarning("4002" + " " + "Bad Request" + " " + item);
            }
            return errors;
        }

        //Get Methods models validations uses here
        public static BadRequestObjectResult InvalidModelStateResponse(IList<ValidationFailure> errorlist_v, ILoggerFactory LoggerFactory)
        {
            var errorlist = errorlist_v.Select(p => p.ErrorMessage).ToList();
            List<Error> errors = ErrorHelperMethods.GenerateFluentValidationErrors(errorlist, LoggerFactory);

            if (errors.Count == 1)
            {
                return new BadRequestObjectResult(new
                {
                    error = new Error
                    {
                        Code = errors.FirstOrDefault().Code,
                        Message = errors.FirstOrDefault().Message,
                        Description = errors.FirstOrDefault().Description,
                    }
                });
            }
            else
            {
                return new BadRequestObjectResult(new
                {
                    errors
                });
            }
        }

        //API Startup.cs uses here
        public static BadRequestObjectResult InvalidModelStateResponse(List<string> errorlist, ILoggerFactory LoggerFactory)
        {
            List<Error> errors = ErrorHelperMethods.GenerateFluentValidationErrors(errorlist, LoggerFactory);

            if (errors.Count == 1)
            {
                return new BadRequestObjectResult(new
                {
                    error = new Error
                    {
                        Code = errors.FirstOrDefault().Code,
                        Message = errors.FirstOrDefault().Message,
                        Description = errors.FirstOrDefault().Description,
                    }
                });
            }
            else
            {
                return new BadRequestObjectResult(new
                {
                    errors
                });
            }
        }


        public static string GetUserId(HttpContext context)
        {
            string result = "-";
            try
            {
                if (context.User.FindFirst("sub") != null)
                    result = context.User.FindFirst("sub").Value;
            }
            catch
            {
            }

            return result;
        }

        public static string CreateErrorTrackingId()
        {
            string result = "-";
            try
            {
                Guid obj = Guid.NewGuid();
                result = obj.ToString();
            }
            catch
            {
            }

            return result;
        }
    }
}
