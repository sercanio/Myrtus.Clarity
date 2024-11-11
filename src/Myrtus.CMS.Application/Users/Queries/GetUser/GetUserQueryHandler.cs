using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Roles.Queries.GetRoleById;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Application.Abstractionss.Repositories;

namespace Myrtus.CMS.Application.Users.Queries.GetUser;

public sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserQueryResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(
            predicate: u => u.Id == request.UserId && u.DeletedOnUtc == null,
            includeSoftDeleted: false,
            include: u => u.Roles);

        if (user is null)
        {
            return Result<GetUserQueryResponse>.NotFound(UserErrors.NotFound.Name);
        }

        var mappedRoles = user.Roles.Select(role =>
            new GetRoleByIdQueryResponse(role.Id, role.Name)).ToList();

        var response = new GetUserQueryResponse(user.Id, user.Email.Value, user.FirstName.Value, user.LastName.Value, mappedRoles);

        return Result.Success(response);
    }
}
