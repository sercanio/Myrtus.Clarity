using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Myrtus.Clarity.Core.Infrastructure.Clock;
using Myrtus.Clarity.Core.Domain.Abstractions;


namespace Myrtus.CMS.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}
