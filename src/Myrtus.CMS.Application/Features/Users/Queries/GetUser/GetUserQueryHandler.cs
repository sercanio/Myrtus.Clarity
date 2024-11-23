using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Application.Features.Roles.Queries.GetRoleById;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Application.Repositories;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetUser
{
    public sealed class GetUserQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserQuery, GetUserQueryResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(
                predicate: u => u.Id == request.UserId && u.DeletedOnUtc == null,
                includeSoftDeleted: false,
                include: u => u.Roles);

            if (user is null)
            {
                return Result<GetUserQueryResponse>.NotFound(UserErrors.NotFound.Name);
            }

            List<GetRoleByIdQueryResponse> mappedRoles = user.Roles.Select(role =>
                new GetRoleByIdQueryResponse(role.Id, role.Name, role.IsDefault)).ToList();

            GetUserQueryResponse response = new(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName, new Collection<GetRoleByIdQueryResponse>(mappedRoles));

            return Result.Success(response);
        }
    }
}
