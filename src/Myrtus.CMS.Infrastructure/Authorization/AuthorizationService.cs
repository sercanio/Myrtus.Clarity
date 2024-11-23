﻿using Microsoft.EntityFrameworkCore;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Infrastructure.Authorization
{
    internal sealed class AuthorizationService(ApplicationDbContext dbContext, ICacheService cacheService)
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
        {
            string cacheKey = $"auth:roles-{identityId}";
            UserRolesResponse? cachedRoles = await _cacheService.GetAsync<UserRolesResponse>(cacheKey);
            if (cachedRoles is not null)
            {
                return cachedRoles;
            }

            UserRolesResponse roles = await _dbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .Select(u => new UserRolesResponse
                {
                    UserId = u.Id,
                    Roles = u.Roles.Where(r => r.DeletedOnUtc == null).ToList()
                })
                .FirstAsync();

            await _cacheService.SetAsync(cacheKey, roles);
            return roles;
        }

        public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
        {
            string cacheKey = $"auth:permissions-{identityId}";
            HashSet<string>? cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey);

            if (cachedPermissions is not null)
            {
                return cachedPermissions;
            }

            List<string> permissions = await _dbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .SelectMany(u => u.Roles.Where(r => r.DeletedOnUtc == null).SelectMany(r => r.Permissions))
                .Select(p => p.Name)
                .ToListAsync();

            HashSet<string> permissionsSet = [.. permissions];

            await _cacheService.SetAsync(cacheKey, permissionsSet);

            return permissionsSet;
        }
    }
}
