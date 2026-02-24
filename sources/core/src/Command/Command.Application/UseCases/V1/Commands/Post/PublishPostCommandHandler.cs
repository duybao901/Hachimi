using Command.Application.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Reaction.ViewModels;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class PublishPostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.PublishPostCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryBase<Posts, Guid> _postRepositoryBase;
    private readonly IRepositoryBase<Tags, Guid> _tagRepositoryBase;
    private readonly IRepositoryBase<ReactionType, Guid> _reactionTypeRepository;

    public PublishPostCommandHandler(
        ICurrentUser currentUser,
        IRepositoryBase<Posts, Guid> postRepositoryBase,
        IRepositoryBase<Tags, Guid> tagRepositoryBase,
        IRepositoryBase<ReactionType, Guid> reactionTypeRepository)
    {
        _postRepositoryBase = postRepositoryBase;
        _tagRepositoryBase = tagRepositoryBase;
        _currentUser = currentUser;
        _reactionTypeRepository = reactionTypeRepository;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.PublishPostCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (request.TagIds.Count() < 4)
        {
            throw new PostException.MinimumTagsRequiredException(4);
        }

        var tags = await _tagRepositoryBase.FindAll(tag => request.TagIds.Contains(tag.Id)).ToListAsync(cancellationToken);
        var slug = SlugGenerator.Generate(request.Title);

        var postId = Guid.NewGuid();
        var reactionTypes = await _reactionTypeRepository.FindAll().ToListAsync(cancellationToken);

        var reactions = reactionTypes.Select(r => new ReactionViewModel
        {
            Id = r.Id,
            Name = r.Name,
            Icon = r.Icon,
            Url = r.Url
        }).ToList();

        var post = Posts.PublishPost(postId, request.Title, slug, request.Content, request.CoverImageUrl, userId, request.TagIds, reactions);
        _postRepositoryBase.Add(post);


        return Result.Success("Publish post success");
    }
}

