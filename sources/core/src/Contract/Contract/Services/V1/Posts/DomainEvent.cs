using Contract.Abstractions.Message;

namespace Contract.Services.V1.Posts;
public static class DomainEvent
{
    public record PostCreatedEvent(Guid IdEvent, Guid Id, string Title, string Slug, string Content, Guid AuthorId, ICollection<Guid> TagIds) : IDomainEvent, ICommand;
    public record PostUpdatedContentEvent(Guid IdEvent, Guid Id, string Title, string Content) : IDomainEvent, ICommand;
    public record PostUpdatedTagEvent(Guid IdEvent, Guid Id, ICollection<Guid> NewTagIds) : IDomainEvent, ICommand;
    public record PostDeletedEvent(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
}
