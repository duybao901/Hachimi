using Contract.Abstractions.Message;

namespace Contract.Services.V1.Identitys;
public static class Query
{
    public record Login(string Email, string Password) : IQuery<Response.LoginTokenResponse>;

    public record Refresh(string? RefreshToken, string Email) : IQuery<Response.RefreshTokenResponse>;
}

