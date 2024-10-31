using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Data;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Users.GetLoggedInUser;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Users.Queries.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository; // Assuming you have a repository for users
    private readonly IUserContext _userContext;

    public GetLoggedInUserQueryHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.IdentityId == _userContext.IdentityId.ToString());

        if (user is null)
        {
            return Result<UserResponse>.NotFound(UserErrors.NotFound.Name);
        }

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Email = user.Email.Value,
            FirstName = user.FirstName.Value,
            LastName = user.LastName.Value
        };

        return Result.Success(userResponse);
    }
}
