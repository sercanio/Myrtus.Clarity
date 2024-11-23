using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Auth;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Features.Accounts.RegisterUser
{
    public sealed class RegisterUserCommandHandler(
        IAuthService authenticationService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IAuthService _authenticationService = authenticationService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            User user = User.Create(
                request.FirstName,
                request.LastName,
                request.Email);

            string? identityId = await _authenticationService.RegisterAsync(
                user,
                request.Password,
                cancellationToken);

            if (identityId is null)
            {
                return Result.NotFound(UserErrors.IdentityIdNotFound.Name);
            }

            user.SetIdentityId(identityId);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
