using System.Data;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using MediatR;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public sealed class GetAllBlogsQueryHandler : IQueryHandler<GetAllBlogsQuery, IEnumerable<BlogResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllBlogsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IEnumerable<BlogResponse>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        string sql = @"
            SELECT
                id AS Id,
                title AS Title,
                slug AS Slug,
                owner_id AS OwnerId,
                created_on_utc AS CreatedOnUtc,
                updated_on_utc AS UpdatedOnUtc
            FROM blogs
            WHERE (@IncludeSoftDeleted = true OR deleted_on_utc IS NULL)
        ";

        var blogs = await connection.QueryAsync<BlogResponse>(sql, new { IncludeSoftDeleted = request.IncludeSoftDeleted });

        return blogs.Any() ? Result.Success(blogs) : Result.Failure<IEnumerable<BlogResponse>>(BlogErrors.NotFound);
    }
}