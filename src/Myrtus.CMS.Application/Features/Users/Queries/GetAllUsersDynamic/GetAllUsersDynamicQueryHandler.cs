using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.Clarity.Application.Repositories;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersDynamic
{
    public sealed class GetAllUsersDynamicQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersDynamicQuery, Result<IPaginatedList<GetAllUsersDynamicQueryResponse>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<IPaginatedList<GetAllUsersDynamicQueryResponse>>> Handle(GetAllUsersDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IPaginatedList<Domain.Users.User> users = await _userRepository.GetAllDynamicAsync(
                    request.DynamicQuery,
                    request.PageIndex,
                    request.PageSize,
                    includeSoftDeleted: false,
                    cancellationToken,
                    user => user.Roles);

                List<GetAllUsersDynamicQueryResponse> mappedUsers = users.Items.Select(user => new GetAllUsersDynamicQueryResponse(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    new Collection<LoggedInUserRolesDto>(
                        user.Roles
                            .Where(role => role.DeletedOnUtc == null)
                            .Select(role => new LoggedInUserRolesDto(role.Id, role.Name))
                            .ToList()
                    )
                )).ToList();

                PaginatedList<GetAllUsersDynamicQueryResponse> paginatedList = new(
                    mappedUsers,
                    users.TotalCount,
                    request.PageIndex,
                    request.PageSize
                );

                return Result.Success<IPaginatedList<GetAllUsersDynamicQueryResponse>>(paginatedList);
            }
            catch (Exception ex)
            {
                return Result.Invalid(new ValidationError
                {
                    Identifier = "GetAllUsersDynamic",
                    ErrorMessage = ex.Message,
                    Severity = ValidationSeverity.Error
                });
            }
        }
    }
}