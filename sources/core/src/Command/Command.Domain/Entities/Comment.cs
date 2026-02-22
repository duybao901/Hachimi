using Command.Domain.Abstractions.Entities;

namespace Command.Domain.Entities;

public class Comment : DomainEntity<Guid>
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }

    public Comment(Guid id, Guid postId, Guid userId, string content)
    {
        Id = id;
        PostId = postId;
        UserId = userId;
        Content = content;
        CreatedOnUtc = DateTimeOffset.UtcNow;
        IsDeleted = false;
    }
}
