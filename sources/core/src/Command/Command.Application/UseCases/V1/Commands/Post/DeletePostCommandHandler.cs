using Command.Domain.Abstractions.Repositories;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;

namespace Command.Application.UserCases.V1.Commands.Post;
public sealed class DeletePostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.DeletePostCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Posts, Guid> _postRepositoryBase;

    public DeletePostCommandHandler(IRepositoryBase<Domain.Entities.Posts, Guid> postRepositoryBase)
    {
        _postRepositoryBase = postRepositoryBase;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepositoryBase.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new PostException.PostNotFoundException(request.Id);

        post.Delete();
        _postRepositoryBase.Remove(post);

        return Result.Success("delete post success");
    }
}
