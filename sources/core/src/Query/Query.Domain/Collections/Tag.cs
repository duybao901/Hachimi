using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Tag)]
public class Tag : Document
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
}