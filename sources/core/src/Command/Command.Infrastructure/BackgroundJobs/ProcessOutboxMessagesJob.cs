using Command.Persistence;
using Command.Persistence.Outbox;
using Contract.Abstractions.Message;
using Contract.Services.V1.Posts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Command.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;

    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All, // Lấy thông tin kiểu dữ liệu($type)
            });

            if (domainEvent is null)
            {
                continue;
            }

            try
            {
                switch (domainEvent.GetType().Name)
                {
                    // Posts
                    case nameof(DomainEvent.PostCreatedEvent):
                        var productCreated = JsonConvert.DeserializeObject<DomainEvent.PostCreatedEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.PostCreatedEvent>(message: productCreated, context.CancellationToken);
                        break;
                    case nameof(DomainEvent.PostUpdatedContentEvent):
                        var productContentUpdated = JsonConvert.DeserializeObject<DomainEvent.PostUpdatedContentEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.PostUpdatedContentEvent>(message: productContentUpdated, context.CancellationToken);
                        break;
                    case nameof(DomainEvent.PostUpdatedTagEvent):
                        var postTagUpdate = JsonConvert.DeserializeObject<DomainEvent.PostUpdatedTagEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.PostUpdatedTagEvent>(message: postTagUpdate, context.CancellationToken);
                        break;
                    case nameof(DomainEvent.PostDeletedEvent):
                        var postDeleted = JsonConvert.DeserializeObject<DomainEvent.PostDeletedEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.PostDeletedEvent>(message: postDeleted, context.CancellationToken);
                        break;
                    case nameof(DomainEvent.PostPublishedEvent):
                        var postPublished = JsonConvert.DeserializeObject<DomainEvent.PostPublishedEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.PostPublishedEvent>(message: postPublished, context.CancellationToken);
                        break;

                    // Tags
                    case nameof(Contract.Services.V1.Tags.DomainEvent.TagCreatedEvent):
                        var tagCreated = JsonConvert.DeserializeObject<Contract.Services.V1.Tags.DomainEvent.TagCreatedEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<Contract.Services.V1.Tags.DomainEvent.TagCreatedEvent>(message: tagCreated, context.CancellationToken);
                        break;
                    case nameof(Contract.Services.V1.Tags.DomainEvent.TagUpdatedEvent):
                        var tagUpdated = JsonConvert.DeserializeObject<Contract.Services.V1.Tags.DomainEvent.TagUpdatedEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<Contract.Services.V1.Tags.DomainEvent.TagUpdatedEvent>(message: tagUpdated, context.CancellationToken);
                        break;
                    case nameof(Contract.Services.V1.Tags.DomainEvent.TagDeletedEvent):
                        var tagDeleted = JsonConvert.DeserializeObject<Contract.Services.V1.Tags.DomainEvent.TagDeletedEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<Contract.Services.V1.Tags.DomainEvent.TagDeletedEvent>(message: tagDeleted, context.CancellationToken);
                        break;
                    default:
                        break;
                }

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
