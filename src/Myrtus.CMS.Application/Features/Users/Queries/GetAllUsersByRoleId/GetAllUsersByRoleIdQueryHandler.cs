using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.CMS.Application.Repositories;
using System.Collections.ObjectModel;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetAllUsersByRoleId
{
    public sealed class GetAllUsersByRoleIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersByRoleIdQuery, Result<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>> Handle(GetAllUsersByRoleIdQuery request, CancellationToken cancellationToken)
        {
            IPaginatedList<Domain.Users.User> users = await _userRepository.GetAllAsync(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                includeSoftDeleted: false,
                predicate: user => user.Roles.Any(role => role.Id == request.RoleId),
                include: user => user.Roles,
                cancellationToken: cancellationToken);

            List<GetAllUsersByRoleIdQueryResponse> mappedUsers = users.Items.Select(
                user => new GetAllUsersByRoleIdQueryResponse(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    new Collection<LoggedInUserRolesDto>(
                        user.Roles.Where(
                            role => role.DeletedOnUtc == null).Select(
                            role => new LoggedInUserRolesDto(role.Id, role.Name)).ToList())
            )).ToList();

            PaginatedList<GetAllUsersByRoleIdQueryResponse> paginatedList = new(
                mappedUsers,
                users.TotalCount,
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>(paginatedList);
        }
    }
}
