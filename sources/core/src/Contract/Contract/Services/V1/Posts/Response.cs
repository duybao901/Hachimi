using Contract.Services.V1.Posts.ViewModels;

namespace Contract.Services.V1.Posts;
public static class Response
{
    public record PostResponse(Guid Id, string Title, string Slug, string Content, PostAuthorViewModel PostAuthor);
    public record PostListResponse(Guid Id, string Title, string Slug, string Content, PostAuthorViewModel PostAuthor);
}
