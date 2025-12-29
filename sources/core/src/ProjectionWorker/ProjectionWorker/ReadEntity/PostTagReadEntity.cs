using ProjectionWorker.Abstractions.Entities;

public class PostTagReadEntity : DomainEntity<Guid>
{
    public Guid PostId { get; private set; }
    public Guid TagId { get; private set; }

    private PostTagReadEntity() { }

    public PostTagReadEntity(Guid postId, Guid tagId)
    {
        PostId = postId;
        TagId = tagId;
    }
}
