using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.StartupExtensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                string API_name = configuration.GetSection("APISettings").GetValue<string>("APIName");
                string API_description = configuration.GetSection("APISettings").GetValue<string>("APIDescription");


                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = API_name,
                    Version = "v1",
                    Description = API_description,
                    TermsOfService = new Uri("http://swagger.io/terms/"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Hakan Taşpınar",
                        Email = string.Empty,
                        Url = new Uri("http://www.hakantaspinar.com/"),
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Url = new Uri("http://www.hakantaspinar.com/p/license.html"),
                    }

                });


                if (configuration.GetSection("APISettings").GetValue<bool>("JWTEnabled"))
                {
                    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                        }
                    });
                }

                //Include only this project XML Comments
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);

                //include that and all other project's swagger xmls. ex: Domain
                string folder = Environment.CurrentDirectory.Replace(Assembly.GetExecutingAssembly().GetName().Name, "");
                string[] list = Directory.GetFiles(folder, "*.XML", SearchOption.AllDirectories);
                if (!string.IsNullOrEmpty(folder))
                {
                    foreach (var name in Directory.GetFiles(folder, "*.XML", SearchOption.AllDirectories))
                    {
                        options.IncludeXmlComments(filePath: name);
                    }
                }

                options.ExampleFilters();
            });

            //get request examples form API Request folders
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("API")).FirstOrDefault();
            services.AddSwaggerExamplesFromAssemblies(assembly);

            return services;

        }
    }
}
