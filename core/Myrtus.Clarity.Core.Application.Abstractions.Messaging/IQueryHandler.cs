using Myrtus.Clarity.Core.Domain.Abstractions;
using MediatR;

namespace Myrtus.Clarity.Core.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
