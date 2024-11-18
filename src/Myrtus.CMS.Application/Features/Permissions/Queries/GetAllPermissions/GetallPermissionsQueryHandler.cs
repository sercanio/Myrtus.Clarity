using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Repositories;

namespace Myrtus.CMS.Application.Features.Permissions.Queries.GetAllPermissions;

public class GetallPermissionsQueryHandler
    : IQueryHandler<GetAllPermissionsQuery, IPaginatedList<GetAllPermissionsQueryResponse>>
{
    private readonly IPermissionRepository _permissionRepository;

    public GetallPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<IPaginatedList<GetAllPermissionsQueryResponse>>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAllAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var mappedPermissions = permissions.Items.Select(permission =>
            new GetAllPermissionsQueryResponse(permission.Id, permission.Feature, permission.Name)).ToList();

        var paginatedList = new PaginatedList<GetAllPermissionsQueryResponse>(
            mappedPermissions,
            permissions.TotalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetAllPermissionsQueryResponse>>(paginatedList);
    }
}
