using Query.Domain.Abstractions;
using Query.Domain.Constants;

namespace Query.Domain.Collections;
[CollectionName(CollectionNames.Author)]
public class Author : Document
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
}
