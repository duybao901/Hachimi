using Contract.Abstractions.Message;

namespace Contract.Services.V1.Identity;
public static class Query
{
    public record Login(string Email, string Password) : IQuery<Response.Authenticated>;

    public record Token(string? AccessToken, string? RefreshToken) : IQuery<Response.Authenticated>;
}

