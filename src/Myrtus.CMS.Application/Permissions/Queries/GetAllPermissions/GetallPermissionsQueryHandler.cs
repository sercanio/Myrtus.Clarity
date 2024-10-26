using System.Data;
using Dapper;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;

namespace Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

public class GetallPermissionsQueryHandler
    : IQueryHandler<GetAllPermissionsQuery,
        IPaginatedList<GroupedPermissionsResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetallPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IPaginatedList<GroupedPermissionsResponse>>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT 
                p.id,
                p.name,
                p.feature
            FROM 
                permissions p
            ORDER BY 
                p.feature
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        var permissions = await connection.QueryAsync<GetAllPermissionsQueryResponse>(
            sql,
            new
            {
                Offset = request.PageIndex * request.PageSize,
                request.PageSize
            });

        const string countSql = "SELECT COUNT(*) FROM permissions";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

        // Group permissions by feature and map to GroupedPermissionsResponse
        var groupedPermissions = permissions
            .GroupBy(p => p.Feature)
            .Select(g => new GroupedPermissionsResponse
            {
                Feature = g.Key,
                Permissions = g.ToList()
            })
            .ToList();

        var paginatedList = new PaginatedList<GroupedPermissionsResponse>(
            groupedPermissions,
            totalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GroupedPermissionsResponse>>(paginatedList);
    }
}

