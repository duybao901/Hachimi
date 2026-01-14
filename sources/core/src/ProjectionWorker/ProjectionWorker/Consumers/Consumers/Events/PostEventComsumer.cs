using Contract.Services.V1.Posts;
using MediatR;
using ProjectionWorker.Abstractions.Messages;
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

    public class PostUpdatedContentEvent : Consumer<DomainEvent.PostUpdatedContentEvent>
    {
        public PostUpdatedContentEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }

    public class PostUpdatedTagEvent : Consumer<DomainEvent.PostUpdatedTagEvent>
    {
        public PostUpdatedTagEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
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

    public class PostPublishedEvent : Consumer<DomainEvent.PostPublishedEvent>
    {
        public PostPublishedEvent(ISender sender, IMongoRepository<EventProjection> eventRepository)
            : base(sender, eventRepository)
        {
        }
    }
}
