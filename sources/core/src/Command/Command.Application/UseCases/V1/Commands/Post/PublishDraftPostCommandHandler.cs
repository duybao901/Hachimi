using Command.Application.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class PublishDraftPostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.PublishDraftPostCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryBase<Domain.Entities.Posts, Guid> _postRepository;

    public PublishDraftPostCommandHandler(ICurrentUser currentUser, IRepositoryBase<Domain.Entities.Posts, Guid> postRepository)
    {
        _currentUser = currentUser;
        _postRepository = postRepository;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.PublishDraftPostCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        var post = await _postRepository.FindSingleAsync(p => p.Id == request.Id)
            ?? throw new PostException.PostNotFoundException(request.Id);
        
        post.PostStatus = Contract.Enumerations.PostStatus.Published;

        post.PublishPost(post.Id);
        _postRepository.Update(post);
        return Result.Success("Publish post success");
    }
}
