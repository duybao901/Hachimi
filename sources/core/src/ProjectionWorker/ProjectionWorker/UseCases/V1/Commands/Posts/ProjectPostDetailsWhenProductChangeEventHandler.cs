using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ProjectionWorker.Abstractions.Repositories;
using ProjectionWorker.Collections;

namespace ProjectionWorker.UseCases.V1.Commands.Posts;

internal class ProjectPostDetailsWhenProductChangeEventHandler :
    ICommandHandler<DomainEvent.PostCreatedEvent>,
    ICommandHandler<DomainEvent.PostUpdatedContentEvent>,
    ICommandHandler<DomainEvent.PostUpdatedTagEvent>,
    ICommandHandler<DomainEvent.PostDeletedEvent>
{
    private readonly IMongoRepository<PostProjection> _postMongoRepository;
    private readonly IMongoRepository<AuthorProjection> _authorMongoRepository;
    private readonly IRepositoryBase<TagReadEntity, Guid> _tagEfRepository;

    //private readonly IMapper _mapper;

    public ProjectPostDetailsWhenProductChangeEventHandler(
        IMongoRepository<PostProjection> postMongoRepository,
        IMongoRepository<TagProjection> tagMongoRepository,
        IMongoRepository<AuthorProjection> authorMongoRepository,
        IRepositoryBase<TagReadEntity, Guid> tagEfRepository)
    {
        _postMongoRepository = postMongoRepository;
        _authorMongoRepository = authorMongoRepository;
        _tagEfRepository = tagEfRepository;
    }

    public async Task<Result> Handle(DomainEvent.PostCreatedEvent request, CancellationToken cancellationToken)
    {
        var tagIds = request.TagIds;
        var tags = await _tagEfRepository.FindAll(t => tagIds.Contains(t.Id)).ToListAsync();

        var tagProjections = tags.Select(t => new TagProjection
        {
            DocumentId = t.Id,
            Name = t.Name,
            Description = t.Description,
            Color = t.Color
        }).ToList();

        AuthorProjection author = await _authorMongoRepository.FindOneAsync(a => a.DocumentId == request.AuthorId);

        var post = new PostProjection
        {
            DocumentId = request.Id,
            Title = request.Title,
            Slug = request.Slug,
            Content = request.Content,
            Author = author,
            Tags = tagProjections
        };

        await _postMongoRepository.InsertOneAsync(post);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostUpdatedContentEvent request, CancellationToken cancellationToken)
    {
        await _postMongoRepository.UpdateOneAsync(
            p => p.DocumentId == request.Id,
            Builders<PostProjection>.Update
                .Set(p => p.Title, request.Title)
                .Set(p => p.Content, request.Content)
                .Set(p => p.ModifiedOnUtc, DateTime.UtcNow)
        );

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostUpdatedTagEvent request, CancellationToken cancellationToken)
    {
        var tagIds = request.NewTagIds;
        var tags = await _tagEfRepository.FindAll(t => tagIds.Contains(t.Id)).ToListAsync();

        var tagProjections = tags.Select(t => new TagProjection
        {
            DocumentId = t.Id,
            Name = t.Name,
            Description = t.Description,
            Color = t.Color
        }).ToList();

        var post = await _postMongoRepository.FindOneAsync(p => p.DocumentId == request.Id);
        if(post is null)
        {
            // In case the post projection does not exist, we just skip updating tags
            return Result.Success();
        }

        post.Tags = tagProjections;
        post.ModifiedOnUtc = DateTime.UtcNow;

        await _postMongoRepository.UpdateOneAsync(
            p => p.DocumentId == request.Id,
            Builders<PostProjection>.Update
                .Set(p => p.Tags, tagProjections)
                .Set(p => p.ModifiedOnUtc, DateTime.UtcNow)
        );
        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostDeletedEvent request, CancellationToken cancellationToken)
    {
        await _postMongoRepository.DeleteOneAsync(p => p.DocumentId == request.Id);
        return Result.Success();
    }
}
