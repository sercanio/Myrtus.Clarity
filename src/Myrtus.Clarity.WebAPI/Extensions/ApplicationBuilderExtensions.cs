using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models.ExternalConnectors;
using Myrtus.Clarity.Application.Features.Accounts.RegisterUser;
using Myrtus.Clarity.Application.Features.Roles.Commands.Create;
using Myrtus.Clarity.Infrastructure;
using Myrtus.Clarity.WebAPI.Middleware;
using System.Text.Json.Serialization;

namespace Myrtus.Clarity.WebAPI.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = scope
                .ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static void UseCustomForbiddenRequestHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ForbiddenResponseMiddleware>();
        }

        public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestContextLoggingMiddleware>();
            return app;
        }

        public static IServiceCollection ConfigureCors(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            string[]? allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials()
                              .WithExposedHeaders("Content-Disposition");
                });
            });
            return services;
        }

        public static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            return services;
        }

        public static IServiceCollection AddValidatiors(this IServiceCollection services)
        {
            services
                .AddValidatorsFromAssemblyContaining<RegisterUserCommand>()
                .AddValidatorsFromAssemblyContaining<CreateRoleValidationhandler>();

            return services;
        }
    }
}
