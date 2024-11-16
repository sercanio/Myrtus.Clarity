using Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public ICollection<GetAllPermissionsQueryResponse> Permissions { get; set; } = [];

    public GetRoleByIdQueryResponse(Guid id, string name, bool isDefault)
    {
        Id = id;
        Name = name;
        IsDefault = isDefault;
    }

    public GetRoleByIdQueryResponse(Guid id, string name, bool isDeault, ICollection<GetAllPermissionsQueryResponse> permissions)
    {
        Id = id;
        Name = name;
        IsDefault = isDeault;
        Permissions = permissions;
    }

    public GetRoleByIdQueryResponse() { }
};