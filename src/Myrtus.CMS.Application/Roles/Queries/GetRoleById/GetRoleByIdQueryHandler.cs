using Ardalis.Result;
using Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Domain.Roles;
using System.Data;

namespace Myrtus.CMS.Application.Roles.Queries.GetRoleById;

public sealed class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetRoleByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<GetRoleByIdQueryResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT 
                r.id AS Id, 
                r.name AS Name,
                r.created_on_utc AS CreatedOnUtc, 
                r.updated_on_utc AS UpdatedOnUtc, 
                r.deleted_on_utc AS DeletedOnUtc,
                p.id AS Id,
                p.name AS Name,
                p.feature AS Feature
            FROM Roles r
            LEFT JOIN role_permissions rp ON rp.role_id = r.id
            LEFT JOIN Permissions p ON p.id = rp.permission_id
            WHERE r.id = @RoleId AND r.deleted_on_utc IS NULL
            """;


        var roleDictionary = new Dictionary<Guid, GetRoleByIdQueryResponse>();

        await connection.QueryAsync<GetRoleByIdQueryResponse, Permission, GetRoleByIdQueryResponse>(
            sql,
            (role, permission) =>
            {
                if (!roleDictionary.TryGetValue(role.Id, out var roleEntry))
                {
                    roleEntry = role;
                    roleEntry.Permissions = new List<Permission>();
                    roleDictionary.Add(role.Id, roleEntry);
                }

                if (permission != null && roleEntry.Permissions.All(p => p.Id != permission.Id))
                {
                    roleEntry.Permissions.Add(permission);
                }

                return roleEntry;
            },
            new { request.RoleId },
            splitOn: "Id"
        );


        if (!roleDictionary.TryGetValue(request.RoleId, out var result))
        {
            return Result<GetRoleByIdQueryResponse>.NotFound(RoleErrors.NotFound.Name);
        }

        return Result.Success(result);
    }
}
