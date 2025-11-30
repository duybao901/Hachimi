using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;

[CollectionName(CollectionNames.Post)]
public class PostProjection : Document
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
