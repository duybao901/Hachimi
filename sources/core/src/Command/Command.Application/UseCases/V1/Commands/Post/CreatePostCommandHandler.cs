using Command.Application.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Microsoft.EntityFrameworkCore;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class CreatePostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.CreatePostCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryBase<Posts, Guid> _postRepositoryBase;
    private readonly IRepositoryBase<Tags, Guid> _tagRepositoryBase;

    public CreatePostCommandHandler(ICurrentUser currentUser, IRepositoryBase<Posts, Guid> postRepositoryBase, IRepositoryBase<Tags, Guid> tagRepositoryBase)
    {
        _postRepositoryBase = postRepositoryBase;
        _tagRepositoryBase = tagRepositoryBase;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.CreatePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (request.TagIds.Count() < 4)
        {
            throw new PostException.MinimumTagsRequiredException(4);
        }

        var tags = await _tagRepositoryBase.FindAll(tag => request.TagIds.Contains(tag.Id)).ToListAsync(cancellationToken);

        var slug = SlugGenerator.Generate(request.Title);

        var isPostEditing = true;
        var post = Posts.CreatePost(Guid.NewGuid(), request.Title, slug, request.Content, request.CoverImageUrl, userId, request.TagIds);
        _postRepositoryBase.Add(post);

        return Result.Success("Create post success");
    }
}
