using ProjectionWorker.Abstractions;
using ProjectionWorker.Constants;

namespace ProjectionWorker.Collections;

[CollectionName(CollectionNames.Tag)]
public class TagProjection : Document
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Color { get; set; }
}