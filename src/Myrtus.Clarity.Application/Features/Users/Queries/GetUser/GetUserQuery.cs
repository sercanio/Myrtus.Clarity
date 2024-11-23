using Myrtus.Clarity.Core.Application.Abstractions.Caching;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetUser
{
    public sealed record GetUserQuery(
       Guid UserId) : ICachedQuery<GetUserQueryResponse>
    {
        public string CacheKey => $"users-{UserId}";

        public TimeSpan? Expiration => null;
    }
}
