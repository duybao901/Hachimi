using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Query.Domain.Abstractions;
public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid DocumentId { get; set; } // SourceMessage Id: ProductID, CustomerID, OrderID

    public DateTimeOffset CreatedOnUtc => Id.CreationTime;

    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
