using Query.Domain.Abstractions;

namespace Query.Domain.Collections;
public class AuthorProjection : Document
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
}
