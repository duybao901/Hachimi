using MassTransit;
using MediatR;
using ProjectionWorker.Abstractions.Repositories;
using ProjectionWorker.Collections;

namespace ProjectionWorker.Abstractions.Messages;
public abstract class Consumer<TMessage> : IConsumer<TMessage>
    where TMessage : class, Contract.Abstractions.Message.IDomainEvent
{
    private readonly ISender _sender;
    private readonly IMongoRepository<EventProjection> _eventRepository;

    protected Consumer(ISender sender, IMongoRepository<EventProjection> eventRepository)
    {
        _sender = sender;
        _eventRepository = eventRepository;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        //var eventProjection = await _eventRepository.FindOneAsync(e => e.EventId == context.Message.IdEvent);
        //if (eventProjection is null)
        //{
            await _sender.Send(context.Message);
        //    eventProjection = new EventProjection()
        //    {
        //        DocumentId = context.Message.Id,
        //        EventId = context.Message.IdEvent,
        //        Name = context.Message.GetType().Name,
        //        Type = context.Message.GetType().Name,
        //    };
        //    await _eventRepository.InsertOneAsync(eventProjection);
        //}
    }
}
