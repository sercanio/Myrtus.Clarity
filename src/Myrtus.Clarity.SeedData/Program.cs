using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Myrtus.Clarity.Application;
using Myrtus.Clarity.Domain;
using Myrtus.Clarity.Infrastructure;
using Myrtus.Clarity.Infrastructure.SeedData;
using Myrtus.Clarity.WebAPI;
using Myrtus.Clarity.WebAPI.Middleware;
using Serilog;

namespace Myrtus.Clarity.SeedData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var app = new ApplicationBuilder(services);
                await ApplyMigrationsAsync(app);
                await app.SeedPermissionsDataAsync();
                await app.SeedRolesDataAsync();
                await app.SeedRolePermissionsDataAsync();
                Guid adminId = await app.SeedUsersDataAsync();
                await app.SeedRoleUserDataAsync(adminId);
            }

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDomain(context.Configuration);
                    services.AddApplication();
                    services.AddInfrastructure(context.Configuration);
                    services.AddWebApi(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(app =>
                    {
                        app.UseMiddleware<ExceptionHandlingMiddleware>();
                        // Other middleware registrations...
                    });
                })
                .UseSerilog((context, loggerConfig) =>
                    loggerConfig.ReadFrom.Configuration(context.Configuration));

        private static async Task ApplyMigrationsAsync(ApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
