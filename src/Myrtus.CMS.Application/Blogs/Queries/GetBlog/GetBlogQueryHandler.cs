using System.Data;
using Dapper;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog;

public sealed class GetBlogQueryHandler : IQueryHandler<GetBlogQuery, GetBlogQueryResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBlogQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<GetBlogQueryResponse>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
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
                u.email as OwnerEmail
            FROM Blogs b
            LEFT JOIN Users u ON b.owner_id = u.Id
            WHERE b.id = @BlogId AND b.deleted_on_utc IS NULL
            """;

        GetBlogQueryResponse? blog = await connection.QuerySingleOrDefaultAsync<GetBlogQueryResponse>(
            sql,
            new { request.BlogId });


        if (blog is null)
        {
            return Result<GetBlogQueryResponse>.NotFound(BlogErrors.NotFound.Name);
        }

        return Result.Success<GetBlogQueryResponse>(blog);
    }
}
