namespace Myrtus.CMS.Domain.Roles;

public sealed class RolePermission
{
    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }
}
