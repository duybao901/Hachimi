using Command.Domain.Abstractions.Aggregates;
using Command.Domain.Abstractions.Entities;
using Contract.Services.V1.Posts.ViewModels;

namespace Command.Domain.Entities;
public class Posts : AggregateRoot<Guid>, IAuditTableEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string? Slug { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public int? ViewCount { get; set; }
    public int? ReadingTimeMinutes { get; set; }
    public Guid AuthorId { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    // Tags
    private readonly List<PostTags> _postTags = new();
    public IReadOnlyCollection<PostTags> PostTags => _postTags;

    public Posts(Guid id, string title, string content, Guid authorId)
    {
        Id = id;
        Title = title;
        Content = content;
        AuthorId = authorId;
    }

    public static Posts CreatePost(Guid id, string title, string content, Guid authorId, List<PostTagViewModel> tags)
    {
        var post = new Posts(id, title, content, authorId);

        post.SetTags(tags);

        post.RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostCreatedEvent(Guid.NewGuid(), id, title, content, authorId, tags));

        return post;
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;

        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostUpdatedEvent(Guid.NewGuid(),
            Id,
            Title,
            Content));
    }

    public void Delete()
    {
        RaiseDomainEvent(new Contract.Services.V1.Posts.DomainEvent.PostDeletedEvent(Guid.NewGuid(), Id));
    }

    public void SetTags(List<PostTagViewModel> Tags)
    {
        _postTags.Clear();

        foreach (var tag in Tags)
        {
            _postTags.Add(new PostTags(Id, tag.Id));
        }
    }
}
