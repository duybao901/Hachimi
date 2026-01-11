using Contract.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Query.Infrastructure.Identity;
internal sealed class CurrentUser : ICurrentUser
{
    public Guid UserId { get; }
    public string Email { get; }

    public CurrentUser(IHttpContextAccessor accessor)
    {
        var headers = accessor.HttpContext?.Request.Headers;

        if (headers is null)
            return;

        var userIdHeader = headers["X-User-Id"].ToString();

        if (Guid.TryParse(userIdHeader, out var userId))
        {
            UserId = userId;
        }

        Email = headers["X-User-Email"].ToString();
    }
}
