using Command.Domain.Abstractions.Aggregates;
using Command.Domain.Abstractions.Entities;
using Contract.Enumerations;
using System.Net.Http.Headers;


namespace Command.Domain.Entities;
public class Posts : AggregateRoot<Guid>, IAuditTableEntity
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
    private readonly List<PostTags> _postTags = new();
    public IReadOnlyCollection<PostTags> PostTags => _postTags.AsReadOnly();


    public Posts(Guid id, string title, string slug, string content, Guid authorId, bool isPostEditing)
    {
        Id = id;
        Title = title;
        Slug = slug;
        Content = content;
        AuthorId = authorId;
        IsPostEditing = isPostEditing;
    }

    public static Posts CreatePost(Guid id, string title, string slug, string content, Guid authorId, List<Guid> tags, bool isPostEditing)
    {
        var post = new Posts(id, title, slug, content, authorId, isPostEditing);

        post.SetTags(tags);

        post.RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostCreatedEvent(Guid.NewGuid(), id, title, slug, content, authorId, tags, isPostEditing));

        return post;
    }

    public void UpdateContent(string title, string content)
    {
        Title = title;
        Content = content;

        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostUpdatedContentEvent(Guid.NewGuid(),
            Id,
            Title,
            Content
            ));
    }

    public void UpdateTags(List<Guid> newTagIds)
    {
        newTagIds = newTagIds
        .Distinct()
        .ToList();

        var oldTagIds = _postTags.Select(x => x.TagId).ToList();

        var removed = oldTagIds.Except(newTagIds).ToList();
        var added = newTagIds.Except(oldTagIds).ToList();

        _postTags.RemoveAll(x => removed.Contains(x.TagId));

        foreach (var tagId in added)
        {
            _postTags.Add(new PostTags(Id, tagId));
        }

        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostUpdatedTagEvent(
            Guid.NewGuid(),
            Id,
            newTagIds
        ));
    }

    public void Delete()
    {
        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostDeletedEvent(Guid.NewGuid(), Id));
    }

    public void SetTags(List<Guid> Tags)
    {
        _postTags.Clear();

        foreach (var tag in Tags)
        {
            _postTags.Add(new PostTags(Id, tag));
        }
    }

    public void PublishPost(Guid Id)
    {
        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostPublishedEvent(Guid.NewGuid(),Id));
    }
}
