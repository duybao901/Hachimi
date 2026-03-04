using Contract.Enumerations;
using ProjectionWorker.Abstractions;
using ProjectionWorker.Constants;

namespace ProjectionWorker.Collections;

[CollectionName(CollectionNames.Post)]
public class PostProjection : Document
{
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string Excerpt { get; set; } = default!;
    public string? CoverImageUrl { get; set; }
    public PostStatus PostStatus { get; set; }
    public bool IsPublished => PostStatus == PostStatus.Published;
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }
    public int LikeCount { get; set; }
    public double TrendingScore { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public int ReadingTimeMinutes { get; set; }
    public bool IsDeleted { get; set; }

    public required AuthorProjection Author { get; set; }
    public required List<TagProjection> Tags { get; set; } = new();
    public required List<ReactionProjection> Reactions { get; set; } = new();
}