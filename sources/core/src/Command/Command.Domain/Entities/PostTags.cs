using Command.Domain.Abstractions.Entities;

public class PostTags : DomainEntity<Guid>
{
    public Guid PostId { get; private set; }
    public Guid TagId { get; private set; }

    private PostTags() { }

    public PostTags(Guid postId, Guid tagId)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        TagId = tagId;
    }
}
