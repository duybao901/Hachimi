using ProjectionWorker.Abstractions.Entities;

public class TagReadEntity : DomainEntity<Guid>, IAuditTableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    private TagReadEntity() { }

    public TagReadEntity(string name, string color)
    {
        Name = name.ToLower().Trim();
        Color = color;
    }
}
