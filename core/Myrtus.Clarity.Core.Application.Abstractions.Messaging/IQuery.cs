using Myrtus.Clarity.Core.Domain.Abstractions;
using MediatR;

namespace Myrtus.Clarity.Core.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
