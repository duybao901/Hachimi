using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Event)]
public class EventProjection : Document
{
    [BsonRepresentation(BsonType.String)]
    public Guid EventId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}
