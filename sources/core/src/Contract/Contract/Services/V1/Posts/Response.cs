using Contract.Enumerations;
using Contract.Services.V1.Posts.ViewModels;
using Contract.Services.V1.Reaction.ViewModels;

namespace Contract.Services.V1.Posts;
public static class Response
{
    public record PostResponse(Guid Id, string Title, string Slug, string Content, PostAuthorViewModel PostAuthor, ICollection<PostTagViewModel> PostTags, ICollection<ReactionViewModel> Reactions, string? CoverImageUrl = null, DateTimeOffset? PublishedAt = null);
    public record PostListResponse(Guid Id, string Title, string Slug, string Content, PostAuthorViewModel PostAuthor, DateTimeOffset? PublishedAt = null);
    public record PostDraftReponse(Guid? Id, string Title, string Content, string UserId, ICollection<Guid> TagIds, PostStatus PostStatus);
}
