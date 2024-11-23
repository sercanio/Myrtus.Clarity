using System.Data;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Application.Features.Roles.Queries.GetAllRoles
{
    public sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetAllRolesQuery, Result<IPaginatedList<GetAllRolesQueryResponse>>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Result<IPaginatedList<GetAllRolesQueryResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            IPaginatedList<Role> roles = await _roleRepository.GetAllAsync(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

            List<GetAllRolesQueryResponse> mappedRoles = roles.Items.Select(role =>
                new GetAllRolesQueryResponse(role.Id, role.Name, role.IsDefault)).ToList();

            PaginatedList<GetAllRolesQueryResponse> paginatedList = new(
                mappedRoles,
                roles.TotalCount,
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllRolesQueryResponse>>(paginatedList);
        }
    }
}
