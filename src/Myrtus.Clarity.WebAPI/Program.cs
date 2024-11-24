using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Myrtus.Clarity.Core.Infrastructure.SignalR.Hubs;
using Myrtus.Clarity.Application;
using Myrtus.Clarity.Domain;
using Myrtus.Clarity.Infrastructure;
using Myrtus.Clarity.WebAPI;
using Myrtus.Clarity.WebAPI.Extensions;
using Myrtus.Clarity.WebAPI.Extensions.SeedData;
using Myrtus.Clarity.WebAPI.OpenApi;
using Serilog;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// Configure MongoDB GuidRepresentation
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

string[]? allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    IConfigurationSection swaggerOAuthSettings = builder.Configuration.GetSection("Swagger:OAuth2");

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
        foreach ((string url, string name) in from ApiVersionDescription description in app.DescribeApiVersions()
                                              let url = $"/swagger/{description.GroupName}/swagger.json"
                                              let name = description.GroupName.ToUpperInvariant()
                                              select (url, name))
        {
            options.SwaggerEndpoint(url, name);
        }

        string? clientId = builder.Configuration["Keycloak:AuthClientId"];
        string? clientSecret = builder.Configuration["Keycloak:AuthClientSecret"];
        string? redirectUri = builder.Configuration["Keycloak:RedirectUri"];

        options.OAuthClientId(clientId);
        options.OAuthClientSecret(clientSecret);
        options.OAuthAppName("Myrtus Clarity Swagger UI");
        options.OAuthUsePkce();

        options.OAuth2RedirectUrl(redirectUri);
    });

    app.ApplyMigrations();

    // Uncomment for seed admin user, roles and permissions
    app.SeedDataAsync().GetAwaiter().GetResult();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

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

app.MapHub<AuditLogHub>("/auditLogHub");

app.Run();

public partial class Program;