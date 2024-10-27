namespace Myrtus.CMS.Application.Users.Update.UpdateUserRoles;

public sealed record UpdateUserRolesCommandResponse
{
    public Guid RoleId { get; init; }
    public Guid UserId { get; init; }

    public UpdateUserRolesCommandResponse(Guid roleId, Guid userId)
    {
        RoleId = roleId;
        UserId = userId;
    }

    private UpdateUserRolesCommandResponse() { }
}
