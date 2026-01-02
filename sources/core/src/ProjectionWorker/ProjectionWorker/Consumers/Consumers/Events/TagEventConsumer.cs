using Contract.Services.V1.Tags;
using MediatR;
using ProjectionWorker.Abstractions.Messages;
using ProjectionWorker.Collections;

namespace ProjectionWorker.Consumers.Consumers.Events;
public class TagEventConsumer
{
    public class TagCreatedEvent : Consumer<DomainEvent.TagCreatedEvent>
    {
        public TagCreatedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }

    public class TagUpdatedEvent : Consumer<DomainEvent.TagUpdatedEvent>
    {
        public TagUpdatedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }

    public class TagDeletedEvent : Consumer<DomainEvent.TagDeletedEvent>
    {
        public TagDeletedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }
}
