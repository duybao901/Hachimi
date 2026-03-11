namespace Query.Application.Abstraction;
public interface ICurrentUser
{
    Guid UserId { get; }
    string Email { get; }
}
