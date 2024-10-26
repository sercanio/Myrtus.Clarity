using System.Data;
using Dapper;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public sealed class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, Result<IPaginatedList<GetBlogQueryResponse>>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllBlogsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IPaginatedList<GetBlogQueryResponse>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT 
                b.id as Id, 
                b.title as Title, 
                b.slug as Slug, 
                b.owner_id as OwnerId, 
                b.created_on_utc as CreatedOnUtc, 
                b.updated_on_utc as UpdatedOnUtc, 
                b.deleted_on_utc as DeletedOnUtc,
                u.Id as OwnerId,
                u.email as OwnerEmail
            FROM Blogs b
            LEFT JOIN Users u ON b.owner_id = u.Id
            WHERE b.deleted_on_utc IS NULL
            ORDER BY b.created_on_utc DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        var blogs = await connection.QueryAsync<GetBlogQueryResponse>(
             sql,
             new
             {
                 Offset = request.PageIndex * request.PageSize, request.PageSize
             });


        const string countSql = "SELECT COUNT(*) FROM Blogs WHERE deleted_on_utc IS NULL";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

        var paginatedList = new PaginatedList<GetBlogQueryResponse>(blogs.ToList(), totalCount, request.PageIndex, request.PageSize);

        return Result.Success<IPaginatedList<GetBlogQueryResponse>>(paginatedList);
    }
}
