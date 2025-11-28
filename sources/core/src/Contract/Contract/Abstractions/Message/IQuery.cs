using Contract.Abstractions.Shared;
using MassTransit;
using MediatR;

namespace Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
