using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Enumerations;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Application.Abstraction;



namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetPublishPostsQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetPublicPostsQuery, PageResult<Response.PostResponse>>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly IMapper _mapper;

    public GetPublishPostsQueryHandler(IMongoRepository<Post> postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<PageResult<Response.PostResponse>>> Handle(Contract.Services.V1.Posts.Query.GetPublicPostsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _postRepository
            .AsQueryable(null);

        // Base filter
        queryable = queryable.Where(p =>
            p.PostStatus == PostStatus.Published);

        // Feed switch

        queryable = request.Feed switch
        {
            "latest" => ApplyLatest(queryable),
            "top_day" => ApplyTop(queryable, 1),
            "top_week" => ApplyTop(queryable, 7),
            "top_month" => ApplyTop(queryable, 30),
            "top_year" => ApplyTop(queryable, 365),
            "top_all" => ApplyTop(queryable, null),
            "top" => ApplyTop(queryable, 7), // Legacy fallback
            _ => ApplyRelevant(queryable)
        };

        // Paging
        var queryResult = await MongoPagingExtensions.ToPageResultAsync(
            queryable,
            request.PageIndex,
            request.PageSize
        );

        var result = queryResult.Map(p => new Response.PostResponse(
                Id: p.DocumentId,
                Title: p.Title,
                Slug: p.Slug,
                Content: p.Content,
                PostAuthor: new PostAuthorViewModel
                {
                    Id = p.Author.DocumentId,
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
            ));

        return Result.Success(result);
    }

    private static IQueryable<Post> ApplyLatest(IQueryable<Post> q)
    {
        return q.OrderByDescending(p => p.PublishedAt);
    }

    private static IQueryable<Post> ApplyTop(IQueryable<Post> q, int? days)
    {
        if (days.HasValue)
        {
            var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days.Value);
            q = q.Where(p => p.PublishedAt >= cutoffDate);
        }

        return q.OrderByDescending(p =>
            p.LikeCount * 1.0 +
            p.CommentCount * 2.0 +
            p.ViewCount * 0.1
        );
    }

    private static IQueryable<Post> ApplyRelevant(IQueryable<Post> query)
    {
        return query.OrderByDescending(p => p.TrendingScore);     
    }
}
