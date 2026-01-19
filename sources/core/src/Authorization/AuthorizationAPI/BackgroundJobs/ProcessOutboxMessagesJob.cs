using AuthorizationAPI.Outbox;
using Contract.Abstractions.Message;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using UserProfileDomainEvent = Contract.Services.V1.UserProfiles.DomainEvent;

namespace AuthorizationAPI.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        if (messages.Count == 0)
            return;

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
                    case nameof(UserProfileDomainEvent.UserRegisterEvent):
                        var userRegisted = JsonConvert.DeserializeObject<UserProfileDomainEvent.UserRegisterEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<UserProfileDomainEvent.UserRegisterEvent>(message: userRegisted, context.CancellationToken);
                        outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                        break;
                    default:
                        _logger.LogError("Unknown domain event type: {DomainEventType}", domainEvent.GetType().Name);
                        break;
                }
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
