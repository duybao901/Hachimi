using Contract.Services.V1.UserProfiles;
using MediatR;
using ProjectionWorker.Abstractions.Messages;
using ProjectionWorker.Abstractions.Repositories;
using ProjectionWorker.Collections;

namespace ProjectionWorker.Consumers.Consumers.Events;
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
