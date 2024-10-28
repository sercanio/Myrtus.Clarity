using System.Data;
using Ardalis.Result;
using Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Roles.Queries.GetRoleById;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Users.GetUser;

public sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserQueryResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            $"""
            SELECT 
                u.id AS {nameof(GetUserQueryResponse.Id)},
                u.email AS {nameof(GetUserQueryResponse.Email)},
                u.first_name AS {nameof(GetUserQueryResponse.FirstName)},
                u.last_name AS {nameof(GetUserQueryResponse.LastName)},
                r.id AS {nameof(GetRoleByIdQueryResponse.Id)},
                r.name AS {nameof(GetRoleByIdQueryResponse.Name)}
            FROM Users u
            LEFT JOIN role_users ru ON u.id = ru.user_id
            LEFT JOIN Roles r ON r.id = ru.role_id
            WHERE u.id = @UserId AND u.deleted_on_utc IS NULL;
            """;

        var userDictionary = new Dictionary<Guid, GetUserQueryResponse>();

        await connection.QueryAsync<GetUserQueryResponse, GetRoleByIdQueryResponse, GetUserQueryResponse>(
            sql,
            (user, role) =>
            {
                if (!userDictionary.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = new GetUserQueryResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = new List<GetRoleByIdQueryResponse>()
                    };
                    userDictionary.Add(userEntry.Id, userEntry);
                }

                if (role != null && role.Id != Guid.Empty && userEntry.Roles.All(r => r.Id != role.Id))
                {
                    userEntry.Roles.Add(new GetRoleByIdQueryResponse
                    {
                        Id = role.Id, 
                        Name = role.Name,
                        Permissions = new List<Permission>()
                    });
                }

                return userEntry;
            },
            new { UserId = request.UserId },
            splitOn: "Id"
        );

        if (!userDictionary.TryGetValue(request.UserId, out var result))
        {
            return Result<GetUserQueryResponse>.NotFound(UserErrors.NotFound.Name);
        }

        return Result.Success(result);
    }

}
