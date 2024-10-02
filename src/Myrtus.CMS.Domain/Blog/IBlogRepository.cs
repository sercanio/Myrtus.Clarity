using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.CMS.Domain.Blogs;
public interface IBlogRepository
{
    Task<Blog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPaginatedList<Blog>> GetAllAsync(bool includeSoftDeleted = false, int pageIndex = 0, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<bool> BlogExistsAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Blog blog);
    void Update(Blog blog);
    void Delete(Blog blog);
}