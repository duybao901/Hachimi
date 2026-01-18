using Contract.Abstractions.Message;

namespace Contract.Services.V1.Posts;
public static class DomainEvent
{
    public record PostCreatedEvent(Guid IdEvent, Guid Id, string Title, string Slug, string Content, string CoverImgUrl, Guid UserId, ICollection<Guid> TagIds) : IDomainEvent, ICommand;
    public record PostUpdatedContentEvent(Guid IdEvent, Guid Id, string Title, string Content, string CoverImageUrl) : IDomainEvent, ICommand;
    public record PostUpdatedTagEvent(Guid IdEvent, Guid Id, ICollection<Guid> NewTagIds) : IDomainEvent, ICommand;
    public record PostSavedContentEvent(Guid IdEvent, Guid Id, string Title, string Content, string CoverImageUrl) : IDomainEvent, ICommand;
    public record PostSavedTagEvent(Guid IdEvent, Guid Id, ICollection<Guid> NewTagIds) : IDomainEvent, ICommand;
    public record PostDeletedEvent(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
    public record PostPublishedEvent(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
}
