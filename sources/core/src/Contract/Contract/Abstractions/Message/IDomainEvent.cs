using MassTransit;

namespace Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IDomainEvent
{
    Guid Id { get; init; }

    Guid IdEvent { get; init; }
}
