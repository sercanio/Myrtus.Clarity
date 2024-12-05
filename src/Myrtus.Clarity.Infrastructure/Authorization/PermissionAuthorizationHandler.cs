using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Azure;
using Myrtus.Clarity.Core.Infrastructure.Authorization;

namespace Myrtus.Clarity.Infrastructure.Authorization
{
    internal sealed class PermissionAuthorizationHandler(IServiceProvider serviceProvider) : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.Identity is not { IsAuthenticated: true })
            {
                return;
            }

            using IServiceScope scope = _serviceProvider.CreateScope();

            AuthorizationService authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

            string identityId = context.User.GetIdentityId();

            HashSet<string> permissions = await authorizationService.GetPermissionsForUserAsync(identityId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
