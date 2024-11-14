using System.Data;
using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Users.GetLoggedInUser;

namespace Myrtus.CMS.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IPaginatedList<GetAllUsersQueryResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IPaginatedList<GetAllUsersQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            includeSoftDeleted: false,
            include: user => user.Roles,
            cancellationToken: cancellationToken);

        var mappedUsers = users.Items.Select(user => new GetAllUsersQueryResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Roles.Where(role => role.DeletedOnUtc == null).Select(role => new LoggedInUserRolesDto(role.Id, role.Name)).ToList()
        )).ToList();

        var paginatedList = new PaginatedList<GetAllUsersQueryResponse>(
            mappedUsers,
            users.TotalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetAllUsersQueryResponse>>(paginatedList);
    }
}
