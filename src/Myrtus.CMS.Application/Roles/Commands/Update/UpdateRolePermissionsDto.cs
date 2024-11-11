using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Application.Roles.Commands.Update;

public sealed class UpdateRolePermissionsDto : Entity
{

    public Guid Id { get; init; }
    public string Feature { get; init; }
    public string Name { get; init; }

    public UpdateRolePermissionsDto(Guid id, string feature, string name)
    {
        Id = id;
        Feature = feature;
        Name = name;
    }

    private UpdateRolePermissionsDto() { }
}
