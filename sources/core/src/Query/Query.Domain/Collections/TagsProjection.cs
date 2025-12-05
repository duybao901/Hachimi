using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Tag)]
public class TagsProjection : Document
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Color { get; set; }

    public int PostCount { get; set; }

    public List<Guid> PostIds { get; set; } = new();
}