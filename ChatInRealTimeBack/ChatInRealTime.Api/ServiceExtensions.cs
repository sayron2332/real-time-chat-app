using Azure;
using Azure.AI.TextAnalytics;
using Azure.Identity;
using ChatInRealTime.Core.Validation.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ChatInRealTime.Api
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAnalysTextAzureService(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(provider =>
            {
                var endpoint = config["Azure:TextAnalyticsAzure:Endpoint"];
                var apiKey = config["Azure:TextAnalyticsAzure:Key"];
                return new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            });
            return services;
        }
        public static IServiceCollection AddAutoFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(SignUpUserValidation).Assembly);
            return services;
        }
        public static IServiceCollection LowerCaseRoutes(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            return services;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtConfig:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"]!)),
                    ValidateIssuerSigningKey = true
                };
            });
            return services;
        }
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatInRealTime Api", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                           }
               });
            });

            return services;
        }
    }
}
