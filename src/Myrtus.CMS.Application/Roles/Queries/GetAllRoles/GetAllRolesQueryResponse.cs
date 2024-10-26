namespace Myrtus.CMS.Application.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public GetAllRolesQueryResponse() { }
};