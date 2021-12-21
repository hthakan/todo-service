
namespace ErrorHandler
{
    public class ErrorHandlerSettings
    {
        public string IndexName { get; set; } = "errors";
        public string CollectionName { get; set; } = "Errors";
        public string DatabaseName { get; set; } = "ErrorDB";
        public string ErrorHandlerType { get; set; }
    }
}
