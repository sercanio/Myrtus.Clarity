namespace Myrtus.Clarity.Application.Features.Roles.Queries.GetAllRoles
{
    public sealed record GetAllRolesQueryResponse(Guid Id, string Name, bool IsDefault);
}