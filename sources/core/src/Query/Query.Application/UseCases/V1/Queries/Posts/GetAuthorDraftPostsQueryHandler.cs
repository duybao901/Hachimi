using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Domain.Abstractions.Repositories;
using Contract.Services.V1.Posts.ViewModels;
using Contract.Services.V1.Reaction.ViewModels;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetAuthorDraftPostsQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetAuthorDraftPostsQuery, List<Response.PostResponse>>
{
    private readonly IMongoRepository<Domain.Collections.Post> _postMongoRepository;

    public GetAuthorDraftPostsQueryHandler(IMongoRepository<Domain.Collections.Post> postMongoRepository)
    {
        _postMongoRepository = postMongoRepository;
    }

    public async Task<Result<List<Response.PostResponse>>> Handle(Contract.Services.V1.Posts.Query.GetAuthorDraftPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postMongoRepository.FindAll(p => p.Author.UserId == request.AuthorId && p.PostStatus != Contract.Enumerations.PostStatus.Published);
        var result = posts.Select(p => new Response.PostResponse(
                p.DocumentId, p.Title, p.Slug, p.Content, 
                PostAuthor: new PostAuthorViewModel
                {
                    Id = p.Author.DocumentId,
                    UserId = p.Author.UserId,
                    Name = p.Author.Name,
                    UserName = p.Author.UserName,
                    Email = p.Author.Email,
                    AvatarUrl = p.Author.AvatarUrl
                },
                PostTags: p.Tags
                    .Select(t => new PostTagViewModel
                    {
                        Id = t.DocumentId,
                        Name = t.Name,
                        Description = t.Description,
                        Color = t.Color
                    })
                    .ToList(),
                Reactions: p.Reactions.Select(r => new Contract.Services.V1.Reaction.ViewModels.ReactionViewModel
                {
                    Id = r.DocumentId,
                    Name = r.Name,
                    Icon = r.Icon,
                    Url = r.Url,
                    Count = r.Count,
                }).ToList(),
                CoverImageUrl: p.CoverImageUrl,
                PublishedAt: p.PublishedAt,
                IsPublished: p.PostStatus == Contract.Enumerations.PostStatus.Published
            )
        ).ToList();

        return Result.Success(result);
    }
}
