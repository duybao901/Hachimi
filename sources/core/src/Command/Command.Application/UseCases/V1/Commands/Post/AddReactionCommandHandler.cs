using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Reaction.ViewModels;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class AddReactionCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.AddReactionCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Posts, Guid> _postRepositoryBase;
    private readonly IRepositoryBase<Domain.Entities.ReactionType, Guid> _reactionTypeRepository;
    private readonly IRepositoryBase<Domain.Entities.PostReaction, Guid> _postReactionRepositoryBase;

    public AddReactionCommandHandler(IRepositoryBase<Domain.Entities.Posts, Guid> postRepositoryBase,
        IRepositoryBase<Domain.Entities.PostReaction, Guid> postReactionRepositoryBase,
        IRepositoryBase<Domain.Entities.ReactionType, Guid> reactionTypeRepository)
    {
        _postRepositoryBase = postRepositoryBase;
        _postReactionRepositoryBase = postReactionRepositoryBase;
        _reactionTypeRepository = reactionTypeRepository;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.AddReactionCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepositoryBase.FindByIdAsync(request.PostId, cancellationToken)
                ?? throw new PostException.PostNotFoundException(request.PostId);

        var reactionType = await _reactionTypeRepository.FindByIdAsync(request.ReactionTypeId, cancellationToken)
            ?? throw new ReactionException.ReactionNotFoundException(request.ReactionTypeId);

        var existingReaction = await _postReactionRepositoryBase.FindSingleAsync(
            r => r.PostId == request.PostId
              && r.UserId == request.UserId
              && r.ReactionTypeId == request.ReactionTypeId,
            cancellationToken);

        if (existingReaction != null)
        {
            _postReactionRepositoryBase.Remove(existingReaction);
            post.RemoveReaction(request.UserId, reactionType.Id);

            return Result.Success("Reaction removed");
        }

        // ADD
        var postReaction = new PostReaction(
            request.PostId,
            request.UserId,
            request.ReactionTypeId
        );

        _postReactionRepositoryBase.Add(postReaction);
        post.AddReaction(request.UserId, reactionType.Id);

        return Result.Success("Reaction added");
    }
}
