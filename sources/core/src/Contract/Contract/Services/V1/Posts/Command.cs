using Contract.Abstractions.Message;

namespace Contract.Services.V1.Posts;
public static class Command
{
    public record CreatePost(string Title, string Content, Guid AuthorId) : ICommand;
}
