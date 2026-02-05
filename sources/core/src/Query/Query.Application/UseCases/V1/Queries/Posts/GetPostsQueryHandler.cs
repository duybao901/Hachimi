using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Enumerations;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using System.Linq;


namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetPostsQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetPostsQuery, PageResult<Response.PostResponse>>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly IMapper _mapper;

    public GetPostsQueryHandler(IMongoRepository<Post> postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<PageResult<Response.PostResponse>>> Handle(Contract.Services.V1.Posts.Query.GetPostsQuery request, CancellationToken cancellationToken)
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
           "top" => ApplyTop(queryable),
           _ => ApplyRelevant(queryable, null)
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
                    .ToList()
            ));

        return Result.Success(result);
    }

    private static IQueryable<Post> ApplyLatest(IQueryable<Post> q)
    {
        return q.OrderByDescending(p => p.PublishedAt);
    }

    private static IQueryable<Post> ApplyTop(IQueryable<Post> q)
    {
        return q
            .Where(p => p.PublishedAt >= DateTime.UtcNow.AddDays(-7))
            .OrderByDescending(p =>
                p.LikeCount * 1.0 +
                p.CommentCount * 2.0
            );
    }

    private IQueryable<Post> ApplyRelevant(
    IQueryable<Post> query,
    Guid? userId)
    {
        return query.OrderByDescending(p => p.PublishedAt);
        //if (userId == null)
        //{
        //    return query.OrderByDescending(p =>
        //        p.LikeCount +
        //        p.CommentCount * 2 +
        //        p.ViewCount * 0.1
        //    );
        //}

        //return query
        //    .OrderByDescending(p =>
        //        //p.Score +                  // score đã pre-calc
        //        (p.AuthorFollowers.Contains(userId) ? 20 : 0)
        //    );
    }
}
