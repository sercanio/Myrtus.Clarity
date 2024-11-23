using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractions.Auth;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Accounts.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAuthService authenticationService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email);

        var identityId = await _authenticationService.RegisterAsync(
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
