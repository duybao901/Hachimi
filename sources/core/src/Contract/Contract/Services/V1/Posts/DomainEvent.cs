using Contract.Abstractions.Message;
using Contract.Services.V1.Posts.ViewModels;

namespace Contract.Services.V1.Posts;
public static class DomainEvent
{
    public record PostCreatedEvent(Guid IdEvent, Guid Id, string Title, string Content, Guid AuthorId, List<PostTagViewModel> Tags) : IDomainEvent, ICommand;
    public record PostUpdatedEvent(Guid IdEvent, Guid Id, string Title, string Content) : IDomainEvent, ICommand;
    public record PostDeletedEvent(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
}
