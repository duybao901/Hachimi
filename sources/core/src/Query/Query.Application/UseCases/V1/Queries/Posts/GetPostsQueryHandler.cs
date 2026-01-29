using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

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
}
