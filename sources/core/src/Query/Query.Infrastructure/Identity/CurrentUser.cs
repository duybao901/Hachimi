using Microsoft.AspNetCore.Http;
using Query.Application.Abstraction;

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

        var userIdHeader = headers["X-User-Id"];

        if (!Guid.TryParse(userIdHeader, out var userId))
        {
            //throw new UnauthorizedAccessException("Invalid user id");
            return;
        }

        UserId = userId;

        Email = headers["X-User-Email"].ToString();
    }
}
