﻿using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.CMS.Application.Features.Roles.Queries.GetAllRoles
{
    public sealed record GetAllRolesQuery(int PageIndex, int PageSize) : IQuery<IPaginatedList<GetAllRolesQueryResponse>>;
}
