using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Accounts.LogInUser
{
    internal sealed class LogInUserCommandHandler(IJwtService jwtService) : ICommandHandler<LogInUserCommand, AccessTokenResponse>
    {
        private readonly IJwtService _jwtService = jwtService;

        public async Task<Result<AccessTokenResponse>> Handle(
            LogInUserCommand request,
            CancellationToken cancellationToken)
        {
            Result<string> result = await _jwtService.GetAccessTokenAsync(
                request.Email,
                request.Password,
                cancellationToken);

            return !result.IsSuccess ? (Result<AccessTokenResponse>)Result.NotFound(UserErrors.InvalidCredentials.Name) : (Result<AccessTokenResponse>)new AccessTokenResponse(result.Value);
        }
    }
}
