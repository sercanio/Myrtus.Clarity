using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Myrtus.Clarity.Application;
using Myrtus.Clarity.Core.Infrastructure.SignalR.Hubs;
using Myrtus.Clarity.Domain;
using Myrtus.Clarity.Infrastructure;
using Myrtus.Clarity.WebAPI;
using Myrtus.Clarity.WebAPI.Extensions;
using Myrtus.Clarity.WebAPI.OpenApi;
using Serilog;
using System.Globalization;
using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// Configure MongoDB GuidRepresentation
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var rateLimitingConfig = builder.Configuration.GetSection("RateLimiting:FixedWindowPolicy");

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, _) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
        }

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
    };

    options.AddPolicy("fixed", httpcontext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpcontext.Connection.RemoteIpAddress?.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = rateLimitingConfig.GetValue<int>("PermitLimit"),
            Window = TimeSpan.FromSeconds(rateLimitingConfig.GetValue<int>("WindowInSeconds")),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = rateLimitingConfig.GetValue<int>("QueueLimit")
        })
    );
});

builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureControllers();
builder.Services.AddValidatiors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Azure AD B2C authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{builder.Configuration["AzureAdB2C:Instance"]}/{builder.Configuration["AzureAdB2C:TenantId"]}/{builder.Configuration["AzureAdB2C:SignUpSignInPolicyId"]}/v2.0/";
        options.Audience = builder.Configuration["AzureAdB2C:ClientId"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AzureAdB2C:ClientId"],
            ValidateLifetime = true
        };

        // settings for SignalR authentication with Azure AD B2C
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/notificationHub") || path.StartsWithSegments("/auditLogHub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
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
    });
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseCustomForbiddenRequestHandler();

app.UseRateLimitExceededHandler();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHub<AuditLogHub>("/auditLogHub");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();

public partial class Program;
