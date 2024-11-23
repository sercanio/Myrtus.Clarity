using Microsoft.Extensions.DependencyInjection;
using Myrtus.Clarity.Core.Application.Abstractions.Behaviors;
using Myrtus.Clarity.Application.Services.Roles;
using Myrtus.Clarity.Application.Services.Users;

namespace Myrtus.Clarity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            AddMediatRBehaviors(services);
            AddApplicationServices(services);

            return services;
        }

        private static void AddMediatRBehaviors(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));

                configuration.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            });
        }

        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>()
                    .AddScoped<IUserService, UserService>();
        }
    }
}
