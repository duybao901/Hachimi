using ProjectionWorker.Abstractions.Entities;
using ProjectionWorker.Enums;

namespace ProjectionWorker.ReadEntity;
public class PostReadEntity : DomainEntity<Guid>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string? CoverImageUrl { get; set; }
    public PostStatus PostStatus { get; set; } = PostStatus.Draft;
    public bool IsPostEditing { get; set; }
    public int? ViewCount { get; set; }
    public int? ReadingTimeMinutes { get; set; }
    public Guid AuthorId { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    // Tags
    private readonly List<PostTagReadEntity> _postTags = new();
    public IReadOnlyCollection<PostTagReadEntity> PostTags => _postTags;
}