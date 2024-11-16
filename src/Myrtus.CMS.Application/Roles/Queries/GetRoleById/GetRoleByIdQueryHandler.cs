using System.Data;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public sealed class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResponse>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<GetRoleByIdQueryResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: role => role.Id == request.RoleId,
            include: role => role.Permissions);

        if (role is null)
        {
            return Result.NotFound(RoleErrors.NotFound.Name);
        }

        var mappedPermissions = role.Permissions.Select(permission =>
            new GetAllPermissionsQueryResponse(permission.Id, permission.Feature, permission.Name)).ToList();

        GetRoleByIdQueryResponse response = new GetRoleByIdQueryResponse(
            role.Id,
            role.Name,
            role.IsDefault,
            mappedPermissions);

        return Result.Success(response);
    }
}
