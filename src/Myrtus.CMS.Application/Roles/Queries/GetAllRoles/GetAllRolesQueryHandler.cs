using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Dapper;
using System.Data;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

namespace Myrtus.CMS.Application.Roles.Queries.GetAllRoles;

public sealed class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<IPaginatedList<GetAllRolesQueryResponse>>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllRolesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IPaginatedList<GetAllRolesQueryResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT 
                r.id, 
                r.name
            FROM roles r
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        var roles = await connection.QueryAsync<GetAllRolesQueryResponse>(
            sql,
            new
            {
                Offset = request.PageIndex * request.PageSize,
                request.PageSize
            }
        );

        const string countSql = "SELECT COUNT(*) FROM Roles";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

        var paginatedList = new PaginatedList<GetAllRolesQueryResponse>(
            roles.ToList(),
            totalCount,
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetAllRolesQueryResponse>>(paginatedList);
    }
}
