using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Contract.Services.V1.Reaction.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Domain.Exceptions;
using Query.Application.Abstraction;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetPostDetailBySlug : IQueryHandler<Contract.Services.V1.Posts.Query.GetPostBySlug, Response.PostResponse>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly ICurrentUser _currentUser;

    public GetPostDetailBySlug(IMongoRepository<Post> postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<Response.PostResponse>> Handle(Contract.Services.V1.Posts.Query.GetPostBySlug request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.FindOneAsync(p => p.Slug == request.Slug)
            ?? throw new PostException.PostNotFoundException(request.Slug);

        return Result.Success(new Response.PostResponse(
            post.DocumentId,
            post.Title,
            post.Slug,
            post.Content.ToString(),
            PostAuthor: new PostAuthorViewModel()
            {
                Id = post.Author.DocumentId,
                Name = post.Author.Name,
                UserName = post.Author.UserName,
                Email = post.Author.Email,
                AvatarUrl = post.Author.AvatarUrl
            },
            PostTags: post.Tags
                    .Select(t => new PostTagViewModel
                    {
                        Id = t.DocumentId,
                        Name = t.Name,
                        Description = t.Description,
                        Color = t.Color
                    })
                    .ToList(),
            Reactions: post.Reactions
                    .Select(r => new ReactionViewModel
                    {
                        Id = r.DocumentId,
                        Name = r.Name,
                        Icon = r.Icon,
                        Url = r.Url,
                        Count = r.Count,
                        IsReactionByCurrentUser = !string.IsNullOrEmpty(_currentUser?.UserId) && r.UserIds.Contains(_currentUser.UserId)
                    })
                    .ToList()
        ));
    }
}
