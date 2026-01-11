namespace Contract.Abstractions;
public interface ICurrentUser
{
    Guid UserId { get; }
    string Email { get; }
}
