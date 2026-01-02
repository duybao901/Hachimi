using Command.Domain.Abstractions.Repositories;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Command.Application.UserCases.V1.Commands.Post;
public sealed class CreatePostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.CreatePostCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Posts, Guid> _postRepositoryBase;

    private readonly IRepositoryBase<Tags, Guid> _tagRepositoryBase;

    public CreatePostCommandHandler(IRepositoryBase<Domain.Entities.Posts, Guid> postRepositoryBase, IRepositoryBase<Tags, Guid> tagRepositoryBase)
    {
        _postRepositoryBase = postRepositoryBase;
        _tagRepositoryBase = tagRepositoryBase;
    }

    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.CreatePostCommand request, CancellationToken cancellationToken)
    {
        //if (request.TagIds.Count() < 4) { 
        //    throw new PostException.MinimumTagsRequiredException(4);
        //}

        var tags = await _tagRepositoryBase.FindAll(tag => request.TagIds.Contains(tag.Id)).ToListAsync(cancellationToken);

        var slug = SlugGenerator.Generate(request.Title);

        var post = Domain.Entities.Posts.CreatePost(Guid.NewGuid(), request.Title, slug, request.Content, request.AuthorId, request.TagIds);
        _postRepositoryBase.Add(post);

        return Result.Success("Create post success");
    }
}
