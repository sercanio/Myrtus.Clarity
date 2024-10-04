using Myrtus.Clarity.Core.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using System.Linq.Expressions;

namespace Myrtus.CMS.Application.Repositories;
public interface IBlogRepository : IRepository<Blog>
{
    Task<Blog?> GetBlogByIdAsync(
            Guid id,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<Blog, object>>[] include);

    Task<bool> BlogExistsByTitleAsync(
        Title title,
        CancellationToken cancellationToken = default);
    
    Task<bool> BlogExistsBySlugAsync(
        Slug slug,
        CancellationToken cancellationToken = default);
}
