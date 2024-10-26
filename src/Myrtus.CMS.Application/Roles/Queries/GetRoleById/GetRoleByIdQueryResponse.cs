using System.Text.Json.Serialization;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQueryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public ICollection<Permission> Permissions { get; set; }

    [JsonConstructor]
    public GetRoleByIdQueryResponse(Guid id, string name, ICollection<Permission> permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions ?? new List<Permission>();
    }

    internal GetRoleByIdQueryResponse() { }
};
