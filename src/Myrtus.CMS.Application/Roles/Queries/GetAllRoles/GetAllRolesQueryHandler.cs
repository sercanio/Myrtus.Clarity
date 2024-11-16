using System.Data;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Repositories;

namespace Myrtus.CMS.Application.Roles.Queries.GetAllRoles;

public sealed class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<IPaginatedList<GetAllRolesQueryResponse>>>
{
    private readonly IRoleRepository _roleRepository;
    public GetAllRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<IPaginatedList<GetAllRolesQueryResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var mappedRoles = roles.Items.Select(role =>
            new GetAllRolesQueryResponse(role.Id, role.Name, role.IsDefault)).ToList();

        var paginatedList = new PaginatedList<GetAllRolesQueryResponse>(
            mappedRoles,
            roles.TotalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetAllRolesQueryResponse>>(paginatedList);
    }
}
