using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.CMS.Application.Repositories;
using System.Collections.ObjectModel;
using System.Data;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetAllUsers
{
    public sealed class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, Result<IPaginatedList<GetAllUsersQueryResponse>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<IPaginatedList<GetAllUsersQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IPaginatedList<Domain.Users.User> users = await _userRepository.GetAllAsync(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                includeSoftDeleted: false,
                include: user => user.Roles,
                cancellationToken: cancellationToken);

            List<GetAllUsersQueryResponse> mappedUsers = users.Items.Select(user => new GetAllUsersQueryResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                new Collection<LoggedInUserRolesDto>(
                            user.Roles.Where(
                                role => role.DeletedOnUtc == null).Select(
                                role => new LoggedInUserRolesDto(role.Id, role.Name)).ToList())
            )).ToList();

            PaginatedList<GetAllUsersQueryResponse> paginatedList = new(
                mappedUsers,
                users.TotalCount,
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllUsersQueryResponse>>(paginatedList);
        }
    }
}
