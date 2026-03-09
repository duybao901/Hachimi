using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ProjectionWorker.Abstractions.Repositories;
using ProjectionWorker.Collections;

namespace ProjectionWorker.UseCases.V1.Commands.Posts;

internal class ProjectPostDetailsWhenProductChangeEventHandler :
    ICommandHandler<DomainEvent.PostCreatedEvent>,
    ICommandHandler<DomainEvent.PostUpdatedContentEvent>,
    ICommandHandler<DomainEvent.PostUpdatedTagEvent>,
    ICommandHandler<DomainEvent.PostDeletedEvent>,
    ICommandHandler<DomainEvent.PostDraftPublishedEvent>,
    ICommandHandler<DomainEvent.PostPublishedEvent>,
    ICommandHandler<DomainEvent.PostReactionToggledEvent>
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
        var tags = await _tagEfRepository.FindAll(t => tagIds.Contains(t.Id)).ToListAsync(cancellationToken: cancellationToken);

        var tagProjections = tags.Select(t => new TagProjection
        {
            DocumentId = t.Id,
            Name = t.Name,
            Description = t.Description,
            Color = t.Color
        }).ToList();

        AuthorProjection author = await _authorMongoRepository.FindOneAsync(a => a.UserId == request.UserId.ToString());
        var reactionProjections = request.Reactions.Select(r => new ReactionProjection
        {
            DocumentId = r.Id,
            Name = r.Name,
            Icon = r.Icon,
            Url = r.Url,
            Count = 0,
            UserIds = [],
        }).ToList();

        var post = new PostProjection
        {
            DocumentId = request.Id,
            Title = request.Title,
            Slug = request.Slug,
            Content = request.Content,
            Author = author,
            Tags = tagProjections,
            CoverImageUrl = request.CoverImgUrl,
            PostStatus = Contract.Enumerations.PostStatus.Draft,
            ViewCount = 0,
            ReadingTimeMinutes = 0,
            CommentCount = 0,
            TrendingScore = 0,
            Reactions = reactionProjections,
            ReactionSummary = new ReactionSummary()
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
                .Set(p => p.CoverImageUrl, request.CoverImageUrl)
                .Set(p => p.ModifiedOnUtc, DateTime.UtcNow)
        );

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostUpdatedTagEvent request, CancellationToken cancellationToken)
    {
        var tagIds = request.NewTagIds;
        var tags = await _tagEfRepository.FindAll(t => tagIds.Contains(t.Id)).ToListAsync(cancellationToken: cancellationToken);

        var tagProjections = tags.Select(t => new TagProjection
        {
            DocumentId = t.Id,
            Name = t.Name,
            Description = t.Description,
            Color = t.Color
        }).ToList();

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

    public async Task<Result> Handle(DomainEvent.PostDraftPublishedEvent request, CancellationToken cancellationToken)
    {
        var post = await _postMongoRepository.FindOneAsync(p => p.DocumentId == request.Id);
        if (post is null)
        {
            // In case the post projection does not exist, we just skip updating tags
            return Result.Success();
        }

        await _postMongoRepository.UpdateOneAsync(
            p => p.DocumentId == request.Id,
            Builders<PostProjection>.Update
                .Set(p => p.ModifiedOnUtc, DateTime.UtcNow)
                .Set(p => p.PostStatus, Contract.Enumerations.PostStatus.Published)
                .Set(p => p.PublishedAt, request.PublishedAt)
        ).ConfigureAwait(true);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostPublishedEvent request, CancellationToken cancellationToken)
    {
        var tagIds = request.TagIds;
        var tags = await _tagEfRepository.FindAll(t => tagIds.Contains(t.Id)).ToListAsync(cancellationToken: cancellationToken);

        var tagProjections = tags.Select(t => new TagProjection
        {
            DocumentId = t.Id,
            Name = t.Name,
            Description = t.Description,
            Color = t.Color
        }).ToList();

        var reactionProjections = request.Reactions.Select(r => new ReactionProjection
        {
            DocumentId = r.Id,
            Name = r.Name,
            Icon = r.Icon,
            Url = r.Url,
            Count = 0,
            UserIds = []
        }).ToList();

        AuthorProjection author = await _authorMongoRepository.FindOneAsync(a => a.UserId == request.UserId.ToString());

        var post = new PostProjection
        {
            DocumentId = request.Id,
            Title = request.Title,
            Slug = request.Slug,
            Content = request.Content,
            Author = author,
            Tags = tagProjections,
            CoverImageUrl = request.CoverImgUrl,
            PostStatus = Contract.Enumerations.PostStatus.Published,
            ViewCount = 0,
            ReadingTimeMinutes = 0,
            CommentCount = 0,
            TrendingScore = 0,       
            Reactions = reactionProjections,
            PublishedAt = request.PublishedAt,
            ReactionSummary = new ReactionSummary()
        };

        await _postMongoRepository.InsertOneAsync(post);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostReactionToggledEvent request, CancellationToken cancellationToken)
    {
        var post = await _postMongoRepository.FindOneAsync(p => p.DocumentId == request.Id);
        if (post is null)
            return Result.Success();

        var summaryField = GetSummaryField(request.ReactionName);
        if (summaryField is null)
            return Result.Success();

        var incValue = request.IsAdded ? 1 : -1;

        var update = Builders<PostProjection>.Update
            .Inc("Reactions.$.Count", incValue)
            .Inc(summaryField, incValue)
            .Inc("ReactionSummary.TotalReactions", incValue);

        if (request.IsAdded)
        {
            update = update.AddToSet("Reactions.$.UserIds", request.UserId.ToString());
        }
        else
        {
            update = update.Pull("Reactions.$.UserIds", request.UserId.ToString());
        }

        await _postMongoRepository.UpdateOneAsync(
            p => p.DocumentId == request.Id
            && p.Reactions.Any(r => r.DocumentId == request.ReactionTypeId),
            update
        );

        return Result.Success();
    }

    private static string? GetSummaryField(string reactName)
    {
        return reactName switch
        {
            "Like" => "ReactionSummary.LikeCount",
            "Unicorn" => "ReactionSummary.UnicornCount",
            "ExplodingHead" => "ReactionSummary.ExplodingHeadCount",
            "RaisedHands" => "ReactionSummary.RaisedHandCount",
            "Fire" => "ReactionSummary.FireCount",
            _ => null
        };
    }
}

