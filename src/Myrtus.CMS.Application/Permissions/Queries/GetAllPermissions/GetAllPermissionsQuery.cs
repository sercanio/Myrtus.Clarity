using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery(
    int PageIndex, 
    int PageSize) : IQuery<IPaginatedList<GetAllPermissionsQueryResponse>>;
