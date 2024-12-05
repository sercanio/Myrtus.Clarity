using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Azure;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Features.Accounts.LogInUser
{
    internal sealed class LogInUserCommandHandler(AzureAdB2CJwtService jwtService) : ICommandHandler<LogInUserCommand, AccessTokenResponse>
    {
        private readonly AzureAdB2CJwtService _jwtService = jwtService;

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
