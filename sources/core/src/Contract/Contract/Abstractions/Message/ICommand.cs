using Contract.Abstractions.Shared;
using MassTransit;
using MediatR;

namespace Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface ICommand : IRequest<Result>
{
}

[ExcludeFromTopology]
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
