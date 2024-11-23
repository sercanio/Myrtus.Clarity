using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser
{
    internal sealed class GetLoggedInUserQueryHandler(IUserRepository userRepository, IUserContext userContext) : IQueryHandler<GetLoggedInUserQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(
                predicate: u => u.IdentityId == _userContext.IdentityId.ToString(),
                includeSoftDeleted: false,
                include: u => u.Roles);

            if (user is null)
            {
                return Result<UserResponse>.NotFound(UserErrors.NotFound.Name);
            }

            List<LoggedInUserRolesDto> mappedRoles = user.Roles.Select(role =>
                new LoggedInUserRolesDto(role.Id, role.Name)).ToList();

            UserResponse userResponse = new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = mappedRoles
            };

            return Result.Success(userResponse);
        }
    }
}
