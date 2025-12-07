using Contract.Abstractions.Message;

namespace Contract.Services.V1.Identity;
public static class Command
{
    public record class RegisterUser(
        string Name,
        string Email,
        string Password,
        string ConfirmPassword
    ) : ICommand;

    public record RevokeToken(string? AccessToken) : ICommand;
}
