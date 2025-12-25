using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Commands.Posts;

internal class ProjectPostDetailsWhenProductChangeEventHandler :
    ICommandHandler<DomainEvent.PostCreatedEvent>,
    ICommandHandler<DomainEvent.PostUpdatedEvent>,
    ICommandHandler<DomainEvent.PostDeletedEvent>
{
    private readonly IMongoRepository<PostProjection> _postRepository;
    private readonly IMongoRepository<TagProjection> _tagRepository;
    private readonly IMongoRepository<AuthorProjection> _authorRepository;
    private readonly IMapper _mapper;

    public ProjectPostDetailsWhenProductChangeEventHandler(
        IMongoRepository<PostProjection> postRepository, 
        IMongoRepository<TagProjection> tagRepository,
        IMongoRepository<AuthorProjection> authorRepository,
        IMapper mapper)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(DomainEvent.PostCreatedEvent request, CancellationToken cancellationToken)
    {
        // Insert Tags if not exist
        foreach (var tag in request.Tags)
        {
            var existingTag = await _tagRepository.FindOneAsync(t => t.DocumentId == tag.Id);
            if (existingTag == null)
            {
                var tagProjection = new TagProjection
                {
                    DocumentId = tag.Id,
                    Name = tag.Name,
                    Slug = tag.Slug,
                    Color = tag.Color,
                };
                await _tagRepository.InsertOneAsync(tagProjection);
            }
        }

        AuthorProjection author = await _authorRepository.FindOneAsync(a => a.DocumentId == request.AuthorId);

        var post = new PostProjection
        {
            DocumentId = request.Id,
            Title = request.Title,
            Slug = request.Slug,
            Content = request.Content,
            Author = author,
            Tags = request.Tags.Select(t => new TagProjection
            {
                DocumentId = t.Id,
                Name = t.Name,
                Slug = t.Slug,
                Color = t.Color,
            }).ToList(),
        };
      
        await _postRepository.InsertOneAsync(post);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostUpdatedEvent request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.FindOneAsync(p => p.DocumentId == request.Id);

        post.Title = request.Title;
        post.Content = request.Content;
        post.ModifiedOnUtc = DateTime.UtcNow;

        await _postRepository.ReplaceOneAsync(post);

        return Result.Success();
    }

    public async Task<Result> Handle(DomainEvent.PostDeletedEvent request, CancellationToken cancellationToken)
    {
        var product = await _postRepository.FindOneAsync(p => p.DocumentId == request.Id) ?? throw new ArgumentNullException();

        await _postRepository.DeleteOneAsync(p => p.Id == product.Id);
        return Result.Success();
    }
}
