using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Post)]
public class Post : Document
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public int ViewCount { get; set; }
    public int ReadingTimeMinutes { get; set; }

    public required Author Author { get; set; }

    public required List<Tag> Tags { get; set; } = new();
}