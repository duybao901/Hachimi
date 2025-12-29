using Contract.Services.V1.Posts;
using MediatR;
using ProjectionWorker.Abstractions.Messages;
using ProjectionWorker.Abstractions.Repositories;
using ProjectionWorker.Collections;

namespace ProjectionWorker.Consumers.Consumers.Events;

public static class PostConsumer
{
    public class PostCreatedEvent : Consumer<DomainEvent.PostCreatedEvent>
    {
        public PostCreatedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }

    public class PostUpdatedEvent : Consumer<DomainEvent.PostUpdatedEvent>
    {
        public PostUpdatedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }

    public class PostDeletedEvent : Consumer<DomainEvent.PostDeletedEvent>
    {
        public PostDeletedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }
}
