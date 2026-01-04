using Command.Domain.Abstractions.Aggregates;
using Command.Domain.Abstractions.Entities;
using MongoDB.Bson;

public class Tags : AggregateRoot<Guid>, IAuditTableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    private Tags() { }

    public Tags(Guid id, string name, string description, string color)
    {
        Id = id;
        Name = name.ToLower().Trim();
        Description = description;
        Color = color;
    }

    public static Tags CreateTag(Guid Id, string Name, string Description, string Color)
    {
        var tag = new Tags(Id, Name, Description, Color);

        tag.RaiseDomainEvent(new Contract.Services.V1.Tags.DomainEvent.TagCreatedEvent(Guid.NewGuid(), Id, Name, Description, Color));

        return tag;
    }
    public void UpdateTag(string name, string description, string color)
    {
        Name = name.ToLower().Trim();
        Description = description;
        Color = color;

        this.RaiseDomainEvent(new Contract.Services.V1.Tags.DomainEvent.TagUpdatedEvent(Guid.NewGuid(), Id, Name, Description, Color));
    }

    public void DeleteTag()
    {
        RaiseDomainEvent(new Contract.Services.V1.Tags.DomainEvent.TagDeletedEvent(Guid.NewGuid(), Id));
    }
}
