using Asp.Versioning.ApiExplorer;
using Myrtus.WebAPI.Extensions;
using Myrtus.WebAPI.OpenApi;
using Myrtus.CMS.Application;
using Myrtus.CMS.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Myrtus.CMS.Domain;
using Myrtus.CMS.WebAPI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var swaggerOAuthSettings = builder.Configuration.GetSection("Swagger:OAuth2");

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(swaggerOAuthSettings["AuthorizationUrl"]),
                TokenUrl = new Uri(swaggerOAuthSettings["TokenUrl"]),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", swaggerOAuthSettings["Scopes:openid"] },
                    { "profile", swaggerOAuthSettings["Scopes:profile"] },
                    { "email", swaggerOAuthSettings["Scopes:email"] }
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "openid", "profile", "email" }
        }
    });
});

builder.Services.AddDomain(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (ApiVersionDescription description in app.DescribeApiVersions())
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }

        var clientId = builder.Configuration["Keycloak:AuthClientId"];
        var clientSecret = builder.Configuration["Keycloak:AuthClientSecret"];
        var redirectUri = builder.Configuration["Keycloak:RedirectUri"];

        options.OAuthClientId(clientId);
        options.OAuthClientSecret(clientSecret);
        options.OAuthAppName("Myrtus CMS Swagger UI");
        options.OAuthUsePkce();

        options.OAuth2RedirectUrl(redirectUri);
    });

    app.ApplyMigrations();

    app.SeedData();
}

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program;
