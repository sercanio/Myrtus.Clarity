using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Repositories.NoSQL;
using Myrtus.Clarity.Application.Services.Auth;
using Myrtus.Clarity.Application.Services.Mailing;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Mailing;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Domain.Abstractions.Mailing;
using Myrtus.Clarity.Core.Infrastructure.Auditing.Services;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Azure;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Azure;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.Infrastructure.Caching;
using Myrtus.Clarity.Core.Infrastructure.Clock;
using Myrtus.Clarity.Core.Infrastructure.Data.Dapper;
using Myrtus.Clarity.Core.Infrastructure.Mailing.MailKit;
using Myrtus.Clarity.Core.Infrastructure.Notification.Services;
using Myrtus.Clarity.Core.Infrastructure.Outbox;
using Myrtus.Clarity.Infrastructure.Authentication;
using Myrtus.Clarity.Infrastructure.Authorization;
using Myrtus.Clarity.Infrastructure.Mailing;
using Myrtus.Clarity.Infrastructure.Repositories;
using Myrtus.Clarity.Infrastructure.Repositories.NoSQL;
using Quartz;

namespace Myrtus.Clarity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null in AddInfrastructure.");
            }

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            AddMailing(services, configuration);

            AddPersistence(services, configuration);

            AddCaching(services, configuration);

            AddAzureAuthentication(services, configuration);

            AddAuthorization(services);

            AddHealthChecks(services, configuration);

            AddApiVersioning(services);

            AddBackgroundJobs(services, configuration);

            AddAuditing(services);

            AddNotification(services);

            AddSignalR(services);

            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Database") ??
                                      throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

            services.AddScoped<IPermissionRepository, PermissionRepository>()
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IRoleRepository, RoleRepository>()
                    .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new SqlConnectionFactory(connectionString));

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            // MongoDB configuration
            string mongoConnectionString = configuration.GetConnectionString("MongoDb") ??
                                           throw new ArgumentNullException(nameof(configuration));
            string mongoDatabaseName = configuration.GetSection("MongoDb:Database").Value ??
                                       throw new ArgumentNullException(nameof(configuration));

            services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString))
                    .AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName))
                    .AddScoped<INoSqlRepository<AuditLog>, NoSqlRepository<AuditLog>>(sp =>
                        new NoSqlRepository<AuditLog>(sp.GetRequiredService<IMongoDatabase>(), "AuditLogs"));
        }

        private static void AddAzureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();

            services.Configure<AzureAdB2COptions>(configuration.GetSection("AzureAdB2C"));

            services.AddTransient<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthService, AuthService>();

            services.AddHttpClient<IJwtService, AzureAdB2CJwtService>((serviceProvider, httpClient) =>
            {
                AzureAdB2COptions azureOptions = serviceProvider.GetRequiredService<IOptions<AzureAdB2COptions>>().Value;

                httpClient.BaseAddress = new Uri(azureOptions.Instance);
            });

            services.AddTransient<AzureAdB2CJwtService>();
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();

            services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>()
                    .AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>()
                    .AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        }

        private static void AddCaching(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Cache") ??
                                      throw new ArgumentNullException(nameof(configuration));

            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

            services.AddSingleton<ICacheService, CacheService>();
        }

        private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddNpgSql(configuration.GetConnectionString("Database")!)
                    .AddRedis(configuration.GetConnectionString("Cache")!)
                    .AddUrlGroup(new Uri(configuration["AzureAdB2C:Instance"]!), HttpMethod.Get, "AzureAdB2C");
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        private static void AddMailing(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailKitMailService>();
            services.AddTransient<IEmailService, EmailService>();
        }

        private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxOptions>(configuration.GetSection("Outbox"))
                    .AddQuartz()
                    .AddQuartzHostedService(options => options.WaitForJobsToComplete = true)
                    .ConfigureOptions<ProcessOutboxMessagesJobSetup>();
        }

        private static void AddAuditing(IServiceCollection services)
        {
            services.AddTransient<IAuditLogService, AuditLogService>();
        }

        private static void AddNotification(IServiceCollection services)
        {
            services.AddTransient<INotificationService, NotificationService>();
        }

        private static void AddSignalR(IServiceCollection services)
        {
            services.AddSignalR();
        }
    }
}
