using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Reaction)]
public class Reaction : Document
{
    public string Name { get; set; } = default!;

    public string Icon { get; set; } = default!;

    public string Url { get; set; } = default!;

    public int Count { get; set; }
    public List<string> UserIds { get; set; } = new();
}
