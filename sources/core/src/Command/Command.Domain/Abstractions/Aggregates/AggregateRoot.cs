using Command.Domain.Abstractions.Entities;
using Contract.Abstractions.Message;

namespace Command.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot<T> : DomainEntity<T>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
