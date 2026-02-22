using Contract.Abstractions.Message;

namespace Contract.Services.V1.Posts;
public static class Command
{
    public record CreatePostCommand(
        string Title,
        string Content,
        Guid UserId,
        List<Guid> TagIds,
        string? CoverImageUrl = null
    ) : ICommand;

    public record UpdatePostCommand(Guid Id, string Title, string Content, List<Guid> TagIds, string CoverImageUrl) : ICommand;
    public record DeletePostCommand(Guid Id) : ICommand;
    public record PublishDraftPostCommand(Guid Id) : ICommand;
    public record SaveDraftPostCommand(string Title, string Content, List<Guid> TagIds, string CoverImageUrl) : ICommand;
    public record PublishPostCommand(string Title, string Content, List<Guid> TagIds, string CoverImageUrl) : ICommand;
    public record AddCommentCommand(Guid Id, Guid UserId, string Content) : ICommand;
    public record AddReactionCommand(Guid PostId, Guid UserId, Guid ReactionTypeId) : ICommand;
}
