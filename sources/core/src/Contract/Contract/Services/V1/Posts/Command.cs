using Contract.Abstractions.Message;

namespace Contract.Services.V1.Posts;
public static class Command
{
    public record CreatePostCommand(
        string Title,
        string Content,
        Guid AuthorId,
        List<Guid> TagIds,
        string? CoverImageUrl = null,
        bool IsPublished = false
    ) : ICommand;

    public record UpdatePostCommand(Guid Id, string Title, string Content, List<Guid> TagIds) : ICommand;
    public record DeletePostCommand(Guid Id) : ICommand;
}
