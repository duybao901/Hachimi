using Command.Domain.Abstractions.Entities;

namespace Command.Domain.Entities;

public sealed class PostReaction: DomainEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid PostId { get; set; }

    public Guid UserId { get; set; }

    public Guid ReactionTypeId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    // Navigation
    public ReactionType ReactionType { get; set; } = default!;

    private PostReaction() { }

    public PostReaction(Guid postId, Guid userId, Guid reactionTypeId)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        UserId = userId;
        ReactionTypeId = reactionTypeId;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public static PostReaction Create(Guid postId, Guid userId, Guid reactionTypeId)
        => new(postId, userId, reactionTypeId);

    public void ChangeReaction(Guid newReactionTypeId)
    {
        ReactionTypeId = newReactionTypeId;
    }
}