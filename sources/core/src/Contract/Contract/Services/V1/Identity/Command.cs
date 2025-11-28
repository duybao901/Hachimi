using Contract.Abstractions.Message;

namespace Contract.Services.V1.Identity;
public static class Command
{
    public record RevokeToken(string? AccessToken) : ICommand;
}
