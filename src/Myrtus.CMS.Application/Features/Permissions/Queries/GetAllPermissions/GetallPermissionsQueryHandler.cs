﻿using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Features.Permissions.Queries.GetAllPermissions
{
    public class GetallPermissionsQueryHandler(IPermissionRepository permissionRepository)
                : IQueryHandler<GetAllPermissionsQuery, IPaginatedList<GetAllPermissionsQueryResponse>>
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task<Result<IPaginatedList<GetAllPermissionsQueryResponse>>> Handle(
            GetAllPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            IPaginatedList<Permission> permissions = await _permissionRepository.GetAllAsync(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

            List<GetAllPermissionsQueryResponse> mappedPermissions = permissions.Items.Select(permission =>
                new GetAllPermissionsQueryResponse(permission.Id, permission.Feature, permission.Name)).ToList();

            PaginatedList<GetAllPermissionsQueryResponse> paginatedList = new(
                mappedPermissions,
                permissions.TotalCount,
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllPermissionsQueryResponse>>(paginatedList);
        }
    }
}
