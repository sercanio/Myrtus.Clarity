using MediatR;
using Ardalis.Result;

namespace Myrtus.Clarity.Core.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
