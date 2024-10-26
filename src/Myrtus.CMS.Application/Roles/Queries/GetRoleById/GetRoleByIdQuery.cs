using Myrtus.Clarity.Core.Application.Abstractions.Caching;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid RoleId) : ICachedQuery<GetRoleByIdQueryResponse>
{
    public string CacheKey => $"roles-{RoleId}";
    public TimeSpan? Expiration => TimeSpan.FromDays(30);
}
