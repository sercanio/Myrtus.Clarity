using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Users.GiveRoleToUser;

public class GiveRoleToUserCommandHandler : ICommandHandler<GiveRoleToUserCommand, GiveRoleToUserCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GiveRoleToUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GiveRoleToUserCommandResponse>> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(
            predicate: user => user.Id == request.UserId,
            include: user => user.Roles,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            return Result.NotFound(UserErrors.NotFound.Name);
        }

        var role = await _roleRepository.GetAsync(
            predicate: role => role.Id == request.RoleId,
            cancellationToken: cancellationToken);

        if (role is null)
        {
            return Result.NotFound(RoleErrors.NotFound.Name);
        }

        User updatedUser = User.AddRole(user, role);

        _userRepository.Update(updatedUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        GiveRoleToUserCommandResponse response = new GiveRoleToUserCommandResponse(
           role.Id,
           user.Id);

        return Result.Success<GiveRoleToUserCommandResponse>(response);
    }
}
