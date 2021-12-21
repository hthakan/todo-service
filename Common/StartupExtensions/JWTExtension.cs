using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Common.StartupExtensions
{
    public static class JWTExtension
    {
        public static IServiceCollection AddJWTConfiguartion(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
                )

                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration.GetSection("JwtAuthentication").GetValue<string>("ValidAudience"),
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetSection("JwtAuthentication").GetValue<string>("ValidIssuer"),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetSection("JwtAuthentication").GetValue<string>("SecurityKey")))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = ctx =>
                        {                          
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = ctx =>
                        {
                            Console.WriteLine("Exception:{0}", ctx.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
