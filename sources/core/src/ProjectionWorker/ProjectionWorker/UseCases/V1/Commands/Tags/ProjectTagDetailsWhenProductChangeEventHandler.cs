using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Tags;
using MongoDB.Driver;
using ProjectionWorker.Collections;

namespace ProjectionWorker.UseCases.V1.Commands.Tags;
public class ProjectTagDetailsWhenProductChangeEventHandler:
    ICommandHandler<DomainEvent.TagCreatedEvent>,
    ICommandHandler<DomainEvent.TagUpdatedEvent>,
    ICommandHandler<DomainEvent.TagDeletedEvent>

{
    private readonly IMongoRepository<TagProjection> _tagMongoRepository;

    public ProjectTagDetailsWhenProductChangeEventHandler(
        IMongoRepository<TagProjection> tagMongoRepository)
    {
        _tagMongoRepository = tagMongoRepository;
    }

    public async Task<Result> Handle(DomainEvent.TagCreatedEvent request, CancellationToken cancellationToken)
    {
        var tag = new TagProjection
        {
            Name = request.Name,
            Slug = request.Slug,
            Color = request.Color,
            DocumentId = request.Id
        };

        await _tagMongoRepository.InsertOneAsync(tag);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.TagUpdatedEvent request, CancellationToken cancellationToken)
    {
        var newTag = new TagProjection
        {
            Name = request.Name,
            Slug = request.Slug,
            Color = request.Color,
            DocumentId = request.Id
        };

        await _tagMongoRepository.UpdateOneAsync(
            p => p.DocumentId == request.Id,
            Builders<TagProjection>.Update
                .Set(p => p.Name, request.Name)
                .Set(p => p.Slug, request.Slug)
                .Set(p => p.Color, request.Color)
                .Set(p => p.ModifiedOnUtc, DateTime.UtcNow)
        ).ConfigureAwait(true);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.TagDeletedEvent request, CancellationToken cancellationToken)
    {
        await _tagMongoRepository.DeleteOneAsync(tag => tag.DocumentId == request.Id);

        return Result.Success();
    }
}
