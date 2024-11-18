using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Features.Users.Queries.GetLoggedInUser;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetAllUsersByRoleId;

public sealed class GetAllUsersByRoleIdQueryHandler : IRequestHandler<GetAllUsersByRoleIdQuery, Result<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersByRoleIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>> Handle(GetAllUsersByRoleIdQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            includeSoftDeleted: false,
            predicate: user => user.Roles.Any(role => role.Id == request.RoleId),
            include: user => user.Roles,
            cancellationToken: cancellationToken);

        var mappedUsers = users.Items.Select(user => new GetAllUsersByRoleIdQueryResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Roles.Where(role => role.DeletedOnUtc == null).Select(role => new LoggedInUserRolesDto(role.Id, role.Name)).ToList()
        )).ToList();

        var paginatedList = new PaginatedList<GetAllUsersByRoleIdQueryResponse>(
            mappedUsers,
            users.TotalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>(paginatedList);
    }
}
