using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Users.GetLoggedInUser;

namespace Myrtus.CMS.Application.Users.Queries.GetAllUsersDynamic
{
    public sealed class GetAllUsersDynamicQueryHandler : IRequestHandler<GetAllUsersDynamicQuery, Result<IPaginatedList<GetAllUsersDynamicQueryResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersDynamicQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<IPaginatedList<GetAllUsersDynamicQueryResponse>>> Handle(GetAllUsersDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllDynamicAsync(
                    request.DynamicQuery,
                    request.PageIndex,
                    request.PageSize,
                    includeSoftDeleted: false,
                    cancellationToken,
                    user => user.Roles);

                var mappedUsers = users.Items.Select(user => new GetAllUsersDynamicQueryResponse(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Roles
                        .Where(role => role.DeletedOnUtc == null)
                        .Select(role => new LoggedInUserRolesDto(role.Id, role.Name))
                        .ToList()
                )).ToList();

                var paginatedList = new PaginatedList<GetAllUsersDynamicQueryResponse>(
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