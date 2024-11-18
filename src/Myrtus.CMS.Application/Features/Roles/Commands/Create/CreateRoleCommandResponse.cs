namespace Myrtus.CMS.Application.Features.Roles.Commands.Create;

public sealed record CreateRoleCommandResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public CreateRoleCommandResponse(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    private CreateRoleCommandResponse() { }
}