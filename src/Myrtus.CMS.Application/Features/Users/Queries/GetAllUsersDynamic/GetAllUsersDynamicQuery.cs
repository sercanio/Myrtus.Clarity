using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetAllUsersDynamic;

public record GetAllUsersDynamicQuery(
        int PageIndex,
        int PageSize,
        DynamicQuery DynamicQuery) : IRequest<Result<IPaginatedList<GetAllUsersDynamicQueryResponse>>>;

