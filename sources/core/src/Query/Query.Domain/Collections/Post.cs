using Contract.Enumerations;
using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Post)]
public class Post : Document
{
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string Excerpt { get; set; } = default!;
    public string CoverImageUrl { get; set; }
    public PostStatus PostStatus { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }
    public int ShareCount { get; set; }
    public double TrendingScore { get; set; }
    public int ReadingTimeMinutes { get; set; }
    public bool IsDeleted { get; set; }
    public required Author Author { get; set; }
    public List<Guid> AuthorFollowers { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
    public List<Reaction> Reactions { get; set; } = [];
    public ReactionSummary ReactionSummary { get; set; }
}

public class ReactionSummary
{
    public int TotalReactions { get; set; }
    public int LikeCount { get; set; }
    public int UnicornCount { get; set; }
    public int ExplodingHeadCount { get; set; }
    public int RaisedHandCount { get; set; }
    public int FireCount { get; set; }
}