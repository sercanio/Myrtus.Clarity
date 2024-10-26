using System.Data;
using Ardalis.Result;
using Dapper;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Users.GetUser;

namespace Myrtus.CMS.Application.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IPaginatedList<GetUserQueryResponse>>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IPaginatedList<GetUserQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT 
                u.id AS Id,
                u.email AS Email,
                u.first_name AS FirstName,
                u.last_name AS LastName
            FROM Users u
            WHERE u.deleted_on_utc IS NULL
            ORDER BY u.created_on_utc DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        var users = await connection.QueryAsync<GetUserQueryResponse>(
             sql,
             new
             {
                 Offset = request.PageIndex * request.PageSize,
                 request.PageSize
             });


        const string countSql = "SELECT COUNT(*) FROM Users WHERE deleted_on_utc IS NULL";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

        var paginatedList = new PaginatedList<GetUserQueryResponse>(users.ToList(), totalCount, request.PageIndex, request.PageSize);

        return Result.Success<IPaginatedList<GetUserQueryResponse>>(paginatedList);
    }
}
