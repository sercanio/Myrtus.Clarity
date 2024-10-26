namespace Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQueryResponse
{
    public Guid Id { get; init; }
    public string Feature {  get; init; }
    public string Name { get; init; }

    private GetAllPermissionsQueryResponse() { }
}

public sealed record GroupedPermissionsResponse
{
    public string Feature { get; init; }
    public List<GetAllPermissionsQueryResponse> Permissions { get; init; }
}
