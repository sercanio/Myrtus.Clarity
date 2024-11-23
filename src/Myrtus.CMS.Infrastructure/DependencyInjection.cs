using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Asp.Versioning;
using Dapper;
using Quartz;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Keycloak;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.Infrastructure.Caching;
using Myrtus.Clarity.Core.Infrastructure.Clock;
using Myrtus.Clarity.Core.Infrastructure.Data.Dapper;
using Myrtus.CMS.Infrastructure.Repositories;
using Myrtus.Clarity.Core.Infrastructure.Outbox;
using Myrtus.CMS.Infrastructure.Authorization;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Infrastructure.Mailing;
using AuthenticationOptions = Myrtus.Clarity.Core.Infrastructure.Authentication.Keycloak.AuthenticationOptions;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using MongoDB.Driver;
using Myrtus.CMS.Application.Repositories.NoSQL;
using Myrtus.CMS.Infrastructure.Repositories.NoSQL;
using Myrtus.CMS.Application.Services.Auth;
using Myrtus.CMS.Application.Services.Mailing;
using Myrtus.CMS.Infrastructure.Authentication;

namespace Myrtus.CMS.Infrastructure
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

            services.AddTransient<IDateTimeProvider, DateTimeProvider>()
                    .AddTransient<IEmailService, EmailService>();

            AddPersistence(services, configuration);

            AddCaching(services, configuration);

            AddAuthentication(services, configuration);

            AddAuthorization(services);

            AddHealthChecks(services, configuration);

            AddApiVersioning(services);

            AddBackgroundJobs(services, configuration);

            AddAuditing(services);

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

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

            services.AddTransient<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthService, AuthService>((serviceProvider, httpClient) =>
            {
                KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
            {
                KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();
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
                    .AddUrlGroup(new Uri(configuration["KeyCloak:BaseUrl"]!), HttpMethod.Get, "keycloak");
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

        private static void AddSignalR(IServiceCollection services)
        {
            services.AddSignalR();
        }
    }
}
