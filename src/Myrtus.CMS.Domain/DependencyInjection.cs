using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Myrtus.CMS.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return configuration == null
                ? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null in AddInfrastructure.")
                : services;
        }
    }
}
