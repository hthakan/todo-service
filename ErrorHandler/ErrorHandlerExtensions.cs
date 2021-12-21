using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ErrorHandler
{
    public static class ErrorHandlerExtensions
    {
        public static IServiceCollection AddErrorHandler(this IServiceCollection services, IConfiguration Configuration)
        {
            var options = new ErrorHandlerSettings();
            Configuration.GetSection(nameof(ErrorHandlerSettings)).Bind(options);
            services.Configure<ErrorHandlerSettings>(Configuration.GetSection(nameof(ErrorHandlerSettings)));

            switch (options.ErrorHandlerType.ToLowerInvariant())
            {
                case "efcore":
                case "mongodb":
                    //services.AddMongoDbErrorHandling(Configuration);
                    break;
                case "elasticsearch":
                    //services.AddElasticsearchErrorHandling(Configuration);
                    break;
                default:
                    throw new Exception($"Event store type '{options.ErrorHandlerType}' is not supported");
            }
            return services;
        }
    }
}
