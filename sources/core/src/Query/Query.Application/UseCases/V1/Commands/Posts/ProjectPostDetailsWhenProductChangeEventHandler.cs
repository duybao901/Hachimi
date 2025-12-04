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

    public ProjectPostDetailsWhenProductChangeEventHandler(IMongoRepository<PostProjection> postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Result> Handle(DomainEvent.PostCreatedEvent request, CancellationToken cancellationToken)
    {
        var product = new PostProjection
        {
            DocumentId = request.Id,
            Title = request.Title,
            Content = request.Content,
        };

        await _postRepository.InsertOneAsync(product);

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
