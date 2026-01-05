using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Tags;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectionWorker.Collections;

namespace ProjectionWorker.UseCases.V1.Commands.Tags;
public class ProjectTagDetailsWhenProductChangeEventHandler:
    ICommandHandler<DomainEvent.TagCreatedEvent>,
    ICommandHandler<DomainEvent.TagUpdatedEvent>,
    ICommandHandler<DomainEvent.TagDeletedEvent>

{
    private readonly IMongoRepository<TagProjection> _tagMongoRepository;
    private readonly IMongoRepository<PostProjection> _postMongoRepository;

    public ProjectTagDetailsWhenProductChangeEventHandler(
        IMongoRepository<TagProjection> tagMongoRepository,
        IMongoRepository<PostProjection> postMongoRepository)
    {
        _tagMongoRepository = tagMongoRepository;
        _postMongoRepository = postMongoRepository;
    }

    public async Task<Result> Handle(DomainEvent.TagCreatedEvent request, CancellationToken cancellationToken)
    {
        var tag = new TagProjection
        {
            Name = request.Name,
            Description = request.Description,
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
            Description = request.Description,
            Color = request.Color,
            DocumentId = request.Id
        };

        await _tagMongoRepository.UpdateOneAsync(
            p => p.DocumentId == request.Id,
            Builders<TagProjection>.Update
                .Set(p => p.Name, request.Name)
                .Set(p => p.Description, request.Description)
                .Set(p => p.Color, request.Color)
                .Set(p => p.ModifiedOnUtc, DateTime.UtcNow)
        ).ConfigureAwait(true);

        var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(
                    new BsonDocument("t.DocumentId", request.Id.ToString())
                )
            };

        await _postMongoRepository.UpdateManyAsync(
            p => p.Tags.Any(t => t.DocumentId == request.Id),
            Builders<PostProjection>.Update
                .Set("Tags.$[t].Name", request.Name)
                .Set("Tags.$[t].Color", request.Color)
                .Set("Tags.$[t].Description", request.Description),
            arrayFilters
            )
            .ConfigureAwait(true);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.TagDeletedEvent request, CancellationToken cancellationToken)
    {
        await _tagMongoRepository.DeleteOneAsync(tag => tag.DocumentId == request.Id);

        return Result.Success();
    }
}
