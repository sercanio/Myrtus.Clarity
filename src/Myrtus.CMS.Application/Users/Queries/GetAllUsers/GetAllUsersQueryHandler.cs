using System.Data;
using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Users.Queries.GetUser;

namespace Myrtus.CMS.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IPaginatedList<GetUserQueryResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IPaginatedList<GetUserQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            include: user => user.Roles,
            cancellationToken: cancellationToken);

        var mappedUsers = users.Items.Select(user => new GetUserQueryResponse(
            user.Id,
            user.Email.Value,
            user.FirstName.Value,
            user.LastName.Value
            )).ToList();

        var paginatedList = new PaginatedList<GetUserQueryResponse>(
            mappedUsers,
            users.TotalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetUserQueryResponse>>(paginatedList);
    }
}
