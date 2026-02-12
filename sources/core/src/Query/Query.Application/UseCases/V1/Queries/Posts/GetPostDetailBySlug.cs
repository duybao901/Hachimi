using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Domain.Exceptions;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetPostDetailBySlug : IQueryHandler<Contract.Services.V1.Posts.Query.GetPostBySlug, Response.PostResponse>
{
    private readonly IMongoRepository<Post> _postRepository;

    public GetPostDetailBySlug(IMongoRepository<Post> postRepository)
    {
        _postRepository = postRepository;
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
            new PostAuthorViewModel()
            {
                Name = post.Author.Name,
                Email = post.Author.Email,
                AvatarUrl = post.Author.AvatarUrl,
            },
            PostTags: post.Tags
                    .Select(t => new PostTagViewModel
                    {
                        Id = t.DocumentId,
                        Name = t.Name,
                        Description = t.Description,
                        Color = t.Color
                    })
                    .ToList()
        ));
    }
}
