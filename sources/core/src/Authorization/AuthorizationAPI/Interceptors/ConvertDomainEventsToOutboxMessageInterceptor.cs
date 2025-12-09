using AuthorizationAPI.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace AuthorizationAPI.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessageInterceptor
    : SaveChangesInterceptor
{
    // Record the Domain Events that occur during data changes
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
       DbContextEventData eventData,
       InterceptionResult<int> result,
       CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // Get the AggregateRoots in the DbContext that contain Domain Events
        var outboxMessages = dbContext.ChangeTracker
            .Entries<Abstractions.Aggregates.AggregateRoot<Guid>>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                // Get the list of Domain Events from the AggregateRoot
                var domainEvents = aggregateRoot.GetDomainEvents();

                // Clear the Domain Events after retrieving to avoid multiple sends
                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                // Convert Domain Event to JSON for storage
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    // -> Add "$type": "Command.Domain.Events.PostCreatedEvent, Command.Domain" to JSON
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
