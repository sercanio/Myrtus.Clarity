﻿using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync(user => user.Id == id, cancellationToken: cancellationToken);
    }
}
