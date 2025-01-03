﻿using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Azure;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.Authorization
{
    internal sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is not { IsAuthenticated: true } ||
               (principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
                principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub)))
            {
                return principal;
            }

            using IServiceScope scope = _serviceProvider.CreateScope();

            AuthorizationService authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

            string identityId = principal.GetIdentityId();

            UserRolesResponse userRoles = await authorizationService.GetRolesForUserAsync(identityId);

            ClaimsIdentity claimsIdentity = new();

            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.UserId.ToString()));

            foreach (Role role in userRoles.Roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            principal.AddIdentity(claimsIdentity);

            return principal;
        }
    }
}
