using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Common.StartupExtensions
{
    public static class ApiVersioningExtension
    {
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer()
                    .AddNewtonsoftJson();
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("x-api-version"); //HTTP Header-Based Versioning
            });

            return services;
        }
    }
}
