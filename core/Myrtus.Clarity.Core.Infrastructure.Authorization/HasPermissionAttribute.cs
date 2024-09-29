using Microsoft.AspNetCore.Authorization;

namespace Myrtus.Clarity.Core.Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
