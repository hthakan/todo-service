using System.Net;

namespace ErrorHandler.Models
{
    public class CustomException : BaseCustomException
    {
        public CustomException(string customCode, string message, string description, int code = (int)HttpStatusCode.InternalServerError, string extraParameter = null, bool isLogged = false, string errorTrackingId = null) : base(customCode, message, description, code, extraParameter, isLogged, errorTrackingId)
        {
        }

        public CustomException(string customCode, string message, string description, bool isLogged) : base(customCode, message, description, 500, null, isLogged, null)
        {
        }
    }
}
