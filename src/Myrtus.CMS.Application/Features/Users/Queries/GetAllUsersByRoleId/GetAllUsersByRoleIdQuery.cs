﻿using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetAllUsersByRoleId
{
    public sealed record GetAllUsersByRoleIdQuery(
        int PageIndex,
        int PageSize,
        Guid RoleId) : IRequest<Result<IPaginatedList<GetAllUsersByRoleIdQueryResponse>>>;
}