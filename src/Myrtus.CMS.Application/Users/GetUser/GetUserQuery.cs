using Myrtus.Clarity.Core.Application.Abstractions.Caching;

namespace Myrtus.CMS.Application.Users.GetUser;

public sealed record GetUserQuery(
   Guid UserId) : ICachedQuery<GetUserQueryResponse>
{
    public string CacheKey => $"users-{UserId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
