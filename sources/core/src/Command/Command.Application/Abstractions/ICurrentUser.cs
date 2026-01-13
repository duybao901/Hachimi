namespace Command.Application.Abstractions;
public interface ICurrentUser
{
    Guid UserId { get; }
    string Email { get; }
}
