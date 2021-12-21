
namespace ErrorHandler.Models
{
    public class CustomErrorResponse
    {
        public Error error { get; set; }
    }
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
