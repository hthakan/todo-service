using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CacheManager;
using Common.Services;
using Common.Settings;
using Common.StartupExtensions;
using ErrorHandler;
using EventBus;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDBManager;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Linq;
using todo_service.AutofacModules;
using todo_service.Mappers;
using todo_service.Requests;
using todo_service.Validations;


namespace todo_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Init Serilog configuration
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
                .Filter.ByExcluding(Matching.FromSource("Couchbase"))
                .Filter.ByExcluding(Matching.FromSource("CAP"))
                .Filter.ByExcluding(Matching.FromSource("Swagger"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.DataProtection"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore"))
                .Filter.ByExcluding(Matching.FromSource("System.Net.Http.HttpClient"))
                .CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static ILoggerFactory _Factory = null;
        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    _Factory.AddSerilog();
                }
                return _Factory;
            }
            set { _Factory = value; }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddCustomConfiguration(Configuration, LoggerFactory)
                    .AddMongoDB(Configuration)
                    .AddCustomApiVersioning()
                    .AddAutoMapperConfiguration()
                    .AddFluentValidation()
                    .AddSwagger(Configuration)
                    .AddErrorHandler(Configuration)
                    .AddCors()
                    .AddValidators()
                    .Configure<APISettings>(Configuration.GetSection("APISettings"))
                    .Configure<EventBusSettings>(Configuration.GetSection("EventBusSettings"));

            if (Configuration.GetSection("APISettings").GetValue<bool>("DCEnabled"))
                services.AddRedisSentinelCache(Configuration);

            if (Configuration.GetSection("APISettings").GetValue<bool>("JWTEnabled"))
                services.AddJWTConfiguartion(Configuration);

            if (Configuration.GetSection("APISettings").GetValue<bool>("EventBusEnabled"))
                services.AddCapEventBus(Configuration);

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"], LoggerFactory));

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IMongoDbContext mongoDbContext, Microsoft.Extensions.Hosting.IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();
            mongoDbContext.Init();


            if (Configuration.GetSection("APISettings").GetValue<bool>("ProfilingEnabled"))
                app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();

            if (Configuration.GetSection("APISettings").GetValue<bool>("DCEnabled"))
                app.UseMiddleware<CustomExceptionMiddlewareWithCache>();
            else
                app.UseMiddleware<CustomExceptionMiddleware>();

            if (Configuration.GetSection("APISettings").GetValue<bool>("JWTEnabled"))
                app.UseAuthentication();

            app.UseStaticFiles();

            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Configuration.GetValue<string>("SwaggerBasePath")}{"v1"}/swagger.json", Configuration.GetSection("APISettings").GetValue<string>("APIName") + " v1");

                //for custom swagger UI please create wwwroot folder and include bellows
                c.InjectStylesheet("theme-material.css");
                c.InjectStylesheet("custom.css");
                c.InjectJavascript("custom.js", "text/javascript");
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            if (Configuration.GetSection("APISettings").GetValue<bool>("JWTEnabled"))
            {
                app.UseAuthorization();
                app.UseAuthentication();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (Configuration.GetSection("APISettings").GetValue<bool>("EventBusEnabled"))
                app.UseCapEventBus();
        }

    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddOptions();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errorlist = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();

                    return ErrorHandler.ErrorHelperMethods.InvalidModelStateResponse(errorlist, loggerFactory);

                };
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new OrderMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
            return services;
        }

    }
}
