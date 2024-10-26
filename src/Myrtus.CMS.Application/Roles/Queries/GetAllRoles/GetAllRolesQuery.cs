using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.CMS.Application.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQuery(int PageIndex, int PageSize, bool? BriefRepresentation) : IQuery<IPaginatedList<GetAllRolesQueryResponse>>;
