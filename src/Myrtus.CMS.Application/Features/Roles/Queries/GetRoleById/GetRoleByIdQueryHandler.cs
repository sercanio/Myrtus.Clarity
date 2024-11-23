using System.Collections.ObjectModel;
using System.Data;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Features.Permissions.Queries.GetAllPermissions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Features.Roles.Queries.GetRoleById
{
    public sealed class GetRoleByIdQueryHandler(IRoleRepository roleRepository) : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResponse>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Result<GetRoleByIdQueryResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            Role? role = await _roleRepository.GetAsync(
                predicate: role => role.Id == request.RoleId,
                include: role => role.Permissions);

            if (role is null)
            {
                return Result.NotFound(RoleErrors.NotFound.Name);
            }

            List<GetAllPermissionsQueryResponse> mappedPermissions = role.Permissions.Select(permission =>
                new GetAllPermissionsQueryResponse(permission.Id, permission.Feature, permission.Name)).ToList();

            GetRoleByIdQueryResponse response = new(
                role.Id,
                role.Name,
                role.IsDefault,
                new Collection<GetAllPermissionsQueryResponse>(mappedPermissions));

            return Result.Success(response);
        }
    }
}
