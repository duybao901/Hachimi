using ProjectionWorker.Abstractions;
using ProjectionWorker.Constants;
using ProjectionWorker.Enums;

namespace ProjectionWorker.Collections;

[CollectionName(CollectionNames.Post)]
public class PostProjection : Document
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string CoverImageUrl { get; set; }
    public PostStatus PostStatus { get; set; } = PostStatus.Draft;
    public bool IsPostEditing { get; set; }
    public int ViewCount { get; set; }
    public int ReadingTimeMinutes { get; set; }

    public required AuthorProjection Author { get; set; }
    public required List<TagProjection> Tags { get; set; } = new();
}