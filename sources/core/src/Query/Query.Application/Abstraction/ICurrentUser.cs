namespace Query.Application.Abstraction;
public interface ICurrentUser
{
    string UserId { get; }
    string Email { get; }
}
