using ProjectionWorker.Abstractions.Entities;

public class TagReadEntity : DomainEntity<Guid>, IAuditTableEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Color { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    private TagReadEntity() { }

    public TagReadEntity(string name, string color)
    {
        Name = name.ToLower().Trim();
        Slug = Name.Replace(" ", "-");
        Color = color;
    }
}
