using System.Data;
using Dapper;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.CMS.Domain.Blogs;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog;

internal sealed class GetBlogQueryHandler : IQueryHandler<GetBlogQuery, BlogResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBlogQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<BlogResponse>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                            SELECT
                                id AS Id,
                                title AS Title,
                                slug AS Slug,
                                created_on_utc AS CreatedOnUtc,
                                updated_on_utc AS UpdatedOnUtc,
                                deleted_on_utc AS DeletedOnUtc
                            FROM blogs
                            WHERE id = @BlogId
                            AND deleted_on_utc IS NULL
                            """;

        BlogResponse? blog = await connection.QueryFirstOrDefaultAsync<BlogResponse>(
            sql,
            new { request.BlogId });

        if (blog is null)
        {
            return Result.Failure<BlogResponse>(BlogErrors.NotFound);
        }

        return blog;
    }
}
