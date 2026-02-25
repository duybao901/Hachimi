using Command.Application.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Reaction.ViewModels;
using Contract.Services.V1.Reaction.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class SaveDraftCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.SaveDraftPostCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryBase<Posts, Guid> _postRepository;
    private readonly IRepositoryBase<Tags, Guid> _tagRepository;
    private readonly IRepositoryBase<ReactionType, Guid> _reactionTypeRepository;

    public SaveDraftCommandHandler(ICurrentUser currentUser,
        IRepositoryBase<Posts, Guid> postRepository,
        IRepositoryBase<Tags, Guid> tagRepository,
        IRepositoryBase<ReactionType, Guid> reactionTypeRepository)
    {
        _currentUser = currentUser;
        _postRepository = postRepository;
        _tagRepository = tagRepository;
        _reactionTypeRepository = reactionTypeRepository;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.SaveDraftPostCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (request.TagIds != null && request.TagIds.Count() < 4)
        {
            throw new PostException.MinimumTagsRequiredException(4);
        }

        var slug = SlugGenerator.Generate(request.Title);
        var reactionTypes = await _reactionTypeRepository.FindAll().ToListAsync(cancellationToken);

        var reactions = reactionTypes.Select(r => new ReactionViewModel
        {
            Id = r.Id,
            Name = r.Name,
            Icon = r.Icon,
            Url = r.Url
        }).ToList();

        var post = Posts.CreatePost(Guid.NewGuid(), request.Title, slug, request.Content, request.CoverImageUrl, userId, request.TagIds, reactions);
        _postRepository.Add(post);

        return Result.Success("Save draft post success");
    }
}
