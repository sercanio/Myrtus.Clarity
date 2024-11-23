using Myrtus.Clarity.Core.WebApi;

namespace Myrtus.Clarity.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IErrorHandlingService, ErrorHandlingService>();

        return services;
    }
}
