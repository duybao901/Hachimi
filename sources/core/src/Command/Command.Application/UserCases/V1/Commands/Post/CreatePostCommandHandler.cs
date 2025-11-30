using Command.Domain.Abstractions.Repositories;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;

namespace Command.Application.UserCases.V1.Commands.Post;
public sealed class CreatePostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.CreatePostCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Post, Guid> _postRepositoryBase;

    public CreatePostCommandHandler(IRepositoryBase<Domain.Entities.Post, Guid> postRepositoryBase)
    {
        _postRepositoryBase = postRepositoryBase;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = Domain.Entities.Post.CreatePost(Guid.NewGuid(), request.Title, request.Content);
        _postRepositoryBase.Add(post);

        return Result.Success("Create post success");
    }
}
