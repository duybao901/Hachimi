using Contract.Services.V1.UserProfiles;
using MediatR;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Infrastructure.Abstractions.Messages;

namespace Query.Infrastructure.MessageBus.Consumers.Events;
public class UserProfileEventComsumer
{
    public class UserRegisterEvent : Consumer<DomainEvent.UserRegisterEvent>
    {
        public UserRegisterEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }
}
