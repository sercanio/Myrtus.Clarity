namespace Myrtus.CMS.Application.Users.GiveRoleToUser;

public sealed record GiveRoleToUserCommandResponse
{
    public Guid RoleId { get; init; }
    public Guid UserId { get; init; }

    public GiveRoleToUserCommandResponse(Guid roleId, Guid userId)
    {
        RoleId = roleId;
        UserId = userId;
    }

    private GiveRoleToUserCommandResponse() { } 
}
