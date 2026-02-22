using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;

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

        // Kiểm tra user đã reaction chưa
        var existingReaction = await _postReactionRepositoryBase.FindSingleAsync(
            r => r.PostId == request.PostId && r.UserId == request.UserId, cancellationToken);

        if (existingReaction != null)
        {
            // Nếu reaction type khác thì update
            if (existingReaction.ReactionTypeId != request.ReactionTypeId)
            {
                existingReaction.ReactionTypeId = request.ReactionTypeId;
                existingReaction.CreatedOnUtc = DateTime.UtcNow;
                _postReactionRepositoryBase.Update(existingReaction);
            }
            // Nếu giống thì có thể return luôn hoặc báo đã reaction
            return Result.Success("Reaction updated");
        }
        else
        {
            // Tạo mới reaction
            var postReaction = new PostReaction(
                request.PostId,
                request.UserId,
                request.ReactionTypeId
            );
            _postReactionRepositoryBase.Add(postReaction);

            // Tăng LikeCount hoặc trường tương ứng nếu cần
            post.LikeCount++;
            _postRepositoryBase.Update(post);
        }

        return Result.Success("Reaction added");
    }
}
