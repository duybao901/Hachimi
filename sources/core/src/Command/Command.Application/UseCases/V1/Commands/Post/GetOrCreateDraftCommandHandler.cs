using Command.Application.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;

namespace Command.Application.UseCases.V1.Commands.Post;
public sealed class GetOrCreateDraftCommandHandler : ICommandHandler<Contract.Services.V1.Posts.Command.GetOrCreateDraftCommand, Response.PostDraftReponse>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryBase<Posts, Guid> _postRepository;
    private readonly IRepositoryBase<Tags, Guid> _tagRepository;

    public GetOrCreateDraftCommandHandler(ICurrentUser currentUser, 
        IRepositoryBase<Domain.Entities.Posts, Guid> postRepository, 
        IRepositoryBase<Tags, Guid> tagRepository)
    {
        _currentUser = currentUser;
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }

    public async Task<Result<Response.PostDraftReponse>> Handle(Contract.Services.V1.Posts.Command.GetOrCreateDraftCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var currentPostDraft = await _postRepository.FindSingleAsync(
            p => p.PostStatus == Contract.Enumerations.PostStatus.Draft & p.AuthorId == userId,
            cancellationToken,
            x => x.PostTags);

        if (currentPostDraft == null)
        {
            var initialCurrentEditPost = new Response.PostDraftReponse(null, "", "", userId.ToString(), [], Contract.Enumerations.PostStatus.Draft);

            return Result.Success(initialCurrentEditPost);
        }

        var tagIds = currentPostDraft.PostTags.Select(pt => pt.TagId).ToList();

        var currentEditPost = new Response.PostDraftReponse(currentPostDraft.Id, currentPostDraft.Title, currentPostDraft.Content, userId.ToString(), tagIds, Contract.Enumerations.PostStatus.Draft);

        return Result.Success(currentEditPost);
    }
}
