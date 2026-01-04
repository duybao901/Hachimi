using Contract.Abstractions.Message;

namespace Contract.Services.V1.Tags;
public static class DomainEvent
{
    public record TagCreatedEvent(Guid IdEvent, Guid Id, string Name, string Description, string Color) : IDomainEvent, ICommand;
    public record TagUpdatedEvent(Guid IdEvent, Guid Id, string Name, string Description, string Color) : IDomainEvent, ICommand;
    public record TagDeletedEvent(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
}
