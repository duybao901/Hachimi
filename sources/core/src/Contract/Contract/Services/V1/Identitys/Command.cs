using Contract.Abstractions.Message;

namespace Contract.Services.V1.Identitys;
public static class Command
{
    public record class RegisterUserCommand(
        string Name,
        string Email,
        string Password,
        string ConfirmPassword
    ) : ICommand;

    public record RevokeToken(string? AccessToken) : ICommand;
    public record Logout(string RefreshToken) : ICommand;
}
