using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Post)]
public class PostProjection : Document
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public int ViewCount { get; set; }
    public int ReadingTimeMinutes { get; set; }

    // Embed Author
    public AuthorViewModel Author { get; set; }

    // Embed list tags
    public List<TagViewModel> Tags { get; set; } = new();
}

public class AuthorViewModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
}

public class TagViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Color { get; set; }
}