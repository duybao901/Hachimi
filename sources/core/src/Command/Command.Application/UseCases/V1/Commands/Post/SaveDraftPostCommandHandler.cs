using Command.Application.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Microsoft.EntityFrameworkCore;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class SaveDraftPostCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.SaveDraftPost>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryBase<Domain.Entities.Posts, Guid> _postRepository;
    private readonly IRepositoryBase<Domain.Entities.Tags, Guid> _tagRepository;

    public SaveDraftPostCommandHandler(ICurrentUser currentUser, 
        IRepositoryBase<Domain.Entities.Posts, Guid> postRepository,
        IRepositoryBase<Domain.Entities.Tags, Guid> tagRepository)
    {
        _currentUser = currentUser;
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }
    public async Task<Result> Handle(Contract.Services.V1.Posts.Command.SaveDraftPost request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var tags = await _tagRepository.FindAll(tag => request.TagIds.Contains(tag.Id)).ToListAsync(cancellationToken);

        var slug = SlugGenerator.Generate(request.Title);

        var post = Domain.Entities.Posts.CreatePost(Guid.NewGuid(), request.Title, slug, request.Content, request.CoverImageUrl, userId, request.TagIds);
        _postRepository.Add(post);
        return Result.Success("Save draft post success");
    }
}
