namespace Myrtus.CMS.Application.Users.TakeRoleFromUser;

public sealed record TakeRoleFromUserCommandResponse
{
    public Guid RoleId { get; init; }
    public Guid UserId { get; init; }

    public TakeRoleFromUserCommandResponse(Guid roleId, Guid userId)
    {
        RoleId = roleId;
        UserId = userId;
    }

    private TakeRoleFromUserCommandResponse() { }   
}
