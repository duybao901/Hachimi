using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Post)]
public class PostProjection : Document
{

    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public int ViewCount { get; set; }
    public int ReadingTimeMinutes { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid AuthorId { get; set; }

    // List of Tag
    [BsonRepresentation(BsonType.String)]
    public List<Guid> TagIds { get; set; } = new();
}