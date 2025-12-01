using Contract.Services.V1.Posts;
using MediatR;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Infrastructure.Abstractions.Messages;

namespace Query.Infrastructure.MessageBus.Consumers.Events;

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
