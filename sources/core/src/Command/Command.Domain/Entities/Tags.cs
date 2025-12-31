using Command.Domain.Abstractions.Entities;

public class Tags : DomainEntity<Guid>, IAuditTableEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Color { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    private Tags() { }

    public Tags(Guid id, string name, string slug, string color)
    {
        Id = id;
        Name = name.ToLower().Trim();
        Slug = slug;
        Color = color;
    }

    public static Tags CreateTag(Guid Id, string Name, string Slug, string Color)
    {
        return new Tags(Id, Name, Slug, Color);
    }
    public void UpdateTag(string name, string slug, string color)
    {
        Name = name.ToLower().Trim();
        Slug = slug;
        Color = color;
    }
}
