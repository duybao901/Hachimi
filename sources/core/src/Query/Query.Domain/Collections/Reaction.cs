using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Reaction)]
public class Reaction : Document
{
    public string Name { get; set; } = default!;

    public string Icon { get; set; } = default!;
}
