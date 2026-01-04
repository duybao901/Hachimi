using ProjectionWorker.Abstractions;
using ProjectionWorker.Constants;

namespace ProjectionWorker.Collections;

[CollectionName(CollectionNames.Tag)]
public class TagProjection : Document
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
}