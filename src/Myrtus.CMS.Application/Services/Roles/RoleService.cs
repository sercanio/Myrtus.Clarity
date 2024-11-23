using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using System.Linq.Expressions;

namespace Myrtus.CMS.Application.Services.Roles
{
    public sealed class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task AddAsync(Role role)
        {
            await _roleRepository.AddAsync(role);
        }

        public void Delete(Role role)
        {
            _roleRepository.Delete(role);
        }

        public void Update(Role role)
        {
            _roleRepository.Update(role);
        }

        public async Task<PaginatedList<Role>> GetAllAsync(
            int index = 0,
            int size = 10,
            bool includeSoftDeleted = false,
            Expression<Func<Role, bool>>? predicate = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Role, object>>[] include)
        {
            IPaginatedList<Role> roles = await _roleRepository.GetAllAsync(
                index,
                size,
                includeSoftDeleted,
                predicate,
                cancellationToken,
                include);

            PaginatedList<Role> paginatedList = new(
                roles.Items,
                roles.TotalCount,
                roles.PageIndex,
                roles.PageSize);

            return paginatedList;
        }

        public async Task<Role> GetAsync(
            Expression<Func<Role, bool>> predicate,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<Role, object>>[] include
            )
        {
            Role? role = await _roleRepository.GetAsync(
                predicate,
                includeSoftDeleted,
                cancellationToken,
                include);

            return role!;
        }
    }
}
