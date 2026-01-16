using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class UpdatePostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.UpdatePostCommand>
{
    private readonly IRepositoryBase<Posts, Guid> _postRepositoryBase;

    public UpdatePostCommandHandler(IRepositoryBase<Posts, Guid> postRepositoryBase)
    {
        _postRepositoryBase = postRepositoryBase;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepositoryBase.FindByIdAsync(
                    request.Id,
                    cancellationToken,
                    x => x.PostTags
                )
            ?? throw new PostException.PostNotFoundException(request.Id);

        post.UpdateContent(request.Title, request.Content, request.CoverImageUrl);
        post.UpdateTags(request.TagIds);

        _postRepositoryBase.Update(post);

        return Result.Success("Update post success");
    }
}
