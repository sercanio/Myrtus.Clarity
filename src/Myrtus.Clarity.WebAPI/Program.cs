using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Myrtus.Clarity.Application;
using Myrtus.Clarity.Core.Application.Abstractions.Module;
using Myrtus.Clarity.Core.Infrastructure.SignalR.Hubs;
using Myrtus.Clarity.Domain;
using Myrtus.Clarity.Infrastructure;
using Myrtus.Clarity.WebAPI;
using Myrtus.Clarity.WebAPI.Extensions;
using Myrtus.Clarity.WebAPI.OpenApi;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;
using Microsoft.OpenApi.Models;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// Configure MongoDB GuidRepresentation
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// Rate Limiting Config
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

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerFileOperationFilter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddTransient<IOperationFilter, SwaggerFileOperationFilter>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            $"{builder.Configuration["AzureAdB2C:Instance"]}/{builder.Configuration["AzureAdB2C:TenantId"]}/{builder.Configuration["AzureAdB2C:SignUpSignInPolicyId"]}/v2.0/";
        options.Audience = builder.Configuration["AzureAdB2C:ClientId"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AzureAdB2C:ClientId"],
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/notificationHub") ||
                     path.StartsWithSegments("/auditLogHub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Domain / Application / Infrastructure / WebApi
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Dynamically load modules
var modulesPath = builder.Environment.IsDevelopment()
    ? Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "modules"))
    : Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "modules"));

var moduleInstances = new List<IClarityModule>();

if (Directory.Exists(modulesPath))
{
    var moduleFiles = Directory.GetFiles(modulesPath, "*.dll", SearchOption.AllDirectories)
        .Where(file => !file.Contains("\\obj\\") && !file.Contains("\\ref\\"));

    foreach (var moduleFile in moduleFiles)
    {
        Console.WriteLine($"Loading module: {moduleFile}");
        try
        {
            var assembly = Assembly.LoadFrom(moduleFile);

            // Add controllers from module
            builder.Services.AddControllers()
                .AddApplicationPart(assembly)
                .AddControllersAsServices();

            // Find all types implementing IClarityModule
            var moduleTypes = assembly.GetTypes()
                .Where(t => typeof(IClarityModule).IsAssignableFrom(t)
                            && !t.IsInterface && !t.IsAbstract);

            // Create & call ConfigureServices
            foreach (var type in moduleTypes)
            {
                var moduleInstance = (IClarityModule)Activator.CreateInstance(type)!;
                moduleInstance.ConfigureServices(builder.Services, builder.Configuration);
                moduleInstances.Add(moduleInstance);
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            Console.WriteLine($"Error loading module {moduleFile}: {ex.LoaderExceptions.FirstOrDefault()?.Message}");
            foreach (var loaderException in ex.LoaderExceptions)
            {
                Console.WriteLine($"Loader Exception: {loaderException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading module {moduleFile}: {ex.Message}");
        }
    }
}

WebApplication app = builder.Build();

// If dev environment, set up swagger endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach ((string url, string name) in
            from ApiVersionDescription description in app.DescribeApiVersions()
            let url = $"/swagger/{description.GroupName}/swagger.json"
            let name = description.GroupName.ToUpperInvariant()
            select (url, name))
        {
            options.SwaggerEndpoint(url, name);
        }
    });
}

// Middlewares
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

// Routes
app.MapControllers();
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHub<AuditLogHub>("/auditLogHub");
app.MapHub<NotificationHub>("/notificationHub");

// **Finally**, invoke Configure on each module
foreach (var moduleInstance in moduleInstances)
{
    moduleInstance.Configure(app);
}

// Run the app
app.Run();

public partial class Program;
