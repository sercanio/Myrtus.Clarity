using Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<GetAllPermissionsQueryResponse> Permissions { get; set; } = [];

    public GetRoleByIdQueryResponse(Guid ıd, string name)
    {
        Id = ıd;
        Name = name;
    }

    public GetRoleByIdQueryResponse(Guid ıd, string name, ICollection<GetAllPermissionsQueryResponse> permissions)
    {
        Id = ıd;
        Name = name;
        Permissions = permissions;
    }
};