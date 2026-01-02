using Command.Domain.Abstractions.Aggregates;
using Command.Domain.Abstractions.Entities;
using MongoDB.Bson;

public class Tags : AggregateRoot<Guid>, IAuditTableEntity
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
        var tag = new Tags(Id, Name, Slug, Color);

        tag.RaiseDomainEvent(new Contract.Services.V1.Tags.DomainEvent.TagCreatedEvent(Guid.NewGuid(), Id, Name, Slug, Color));

        return tag;
    }
    public void UpdateTag(string name, string slug, string color)
    {
        Name = name.ToLower().Trim();
        Slug = slug;
        Color = color;

        this.RaiseDomainEvent(new Contract.Services.V1.Tags.DomainEvent.TagUpdatedEvent(Guid.NewGuid(), Id, Name, Slug, Color));
    }

    public void DeleteTag()
    {
        RaiseDomainEvent(new Contract.Services.V1.Tags.DomainEvent.TagDeletedEvent(Guid.NewGuid(), Id));
    }
}
