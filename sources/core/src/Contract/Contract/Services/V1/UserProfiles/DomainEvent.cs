using Contract.Abstractions.Message;

namespace Contract.Services.V1.UserProfiles;
public class DomainEvent
{
    public record UserRegisterEvent(Guid IdEvent, Guid Id, string Name, string UserName, string Email) : IDomainEvent, ICommand;
    public record UserUpdatedEvent(Guid IdEvent, Guid Id, string Name, string UserName, string Email) : IDomainEvent, ICommand;
    public record UserDeletedEvent(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
}
