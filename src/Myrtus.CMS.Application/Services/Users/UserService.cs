using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;
using System.Linq.Expressions;

namespace Myrtus.CMS.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }

    public void Delete(User user)
    {
        _userRepository.Delete(user);
    }


    public void Update(User user)
    {
        _userRepository.Update(user);
    }

    public async Task<PaginatedList<User>> GetAllAsync(
        int index = 0,
        int size = 10,
        bool includeSoftDeleted = false,
        Expression<Func<User, bool>>? predicate = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<User, object>>[] include)
    {
        IPaginatedList<User> users = await _userRepository.GetAllAsync(
            index,
            size,
            includeSoftDeleted,
            predicate,
            cancellationToken,
            include);

        PaginatedList<User> paginatedList = new(
            users.Items,
            users.TotalCount,
            users.PageIndex,
            users.PageSize);

        return paginatedList;
    }

    public async Task<User> GetAsync(Expression<Func<User, bool>> predicate, bool includeSoftDeleted = false, CancellationToken cancellationToken = default, params Expression<Func<User, object>>[] include)
    {
        User? role = await _userRepository.GetAsync(
            predicate,
            includeSoftDeleted,
            cancellationToken,
            include);

        return role!;
    }


    public async Task<User> GetUserByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        User? user = await _userRepository.GetAsync(
            predicate: user => user.Id == id,
            cancellationToken: cancellationToken);
        return user!;
    }
}
