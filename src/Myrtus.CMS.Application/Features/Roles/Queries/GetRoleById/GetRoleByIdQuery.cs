﻿using Myrtus.Clarity.Core.Application.Abstractions.Caching;

namespace Myrtus.CMS.Application.Features.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid RoleId) : ICachedQuery<GetRoleByIdQueryResponse>
{
    public string CacheKey => $"roles-{RoleId}";
    public TimeSpan? Expiration => null;
}
