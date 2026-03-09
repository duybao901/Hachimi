using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Contract.Services.V1.Posts.ViewModels;
using Contract.Services.V1.Reaction.ViewModels;
using Query.Domain.Exceptions;

namespace Query.Application.UseCases.V1.Queries.Posts;

public sealed class GetPostByIdQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetPostByIdQuery, Response.PostResponse>
{
    private readonly IMongoRepository<Post> _postRepository;

    public GetPostByIdQueryHandler(IMongoRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Result<Response.PostResponse>> Handle(Contract.Services.V1.Posts.Query.GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.FindOneAsync(p => p.DocumentId == request.PostId) ??
            throw new PostException.PostNotFoundException(request.PostId);

        var response = new Response.PostResponse(
            post.DocumentId,
            post.Title,
            post.Slug,
            post.Content,
            new PostAuthorViewModel
            {
                Id = post.Author.DocumentId,
                UserId = post.Author.UserId,
                Name = post.Author.Name,
                UserName = post.Author.UserName,
                Email = post.Author.Email,
                AvatarUrl = post.Author.AvatarUrl
            },
            post.Tags.Select(t => new PostTagViewModel
            {
                Id = t.DocumentId,
                Name = t.Name,
                Description = t.Description,
                Color = t.Color
            }).ToList(),
            post.Reactions.Select(r => new ReactionViewModel
            {
                Id = r.DocumentId,
                Name = r.Name,
                Icon = r.Icon,
                Url = r.Url,
                Count = r.Count
            }).ToList(),
            post.CoverImageUrl,
            post.PublishedAt,
            post.PostStatus == Contract.Enumerations.PostStatus.Published
        );

        return Result.Success(response);
    }
}
