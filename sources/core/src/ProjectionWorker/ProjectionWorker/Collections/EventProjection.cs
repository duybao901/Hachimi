using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectionWorker.Abstractions;
using ProjectionWorker.Constants;

namespace ProjectionWorker.Collections;

[CollectionName(CollectionNames.Event)]
public class EventProjection : Document
{
    [BsonRepresentation(BsonType.String)]
    public Guid EventId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}
