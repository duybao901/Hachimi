using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Enumerations;
using Contract.Extensions;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using MongoDB.Driver.Linq;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using System.Linq.Expressions;

namespace Query.Application.UseCases.V1.Queries.Posts;
public class GetListPostsQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetListPostsQuery, PageResult<Response.PostListResponse>>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly IMongoRepository<Author> _authorRepository;
    private readonly IMapper _mapper;

    public GetListPostsQueryHandler(IMongoRepository<Post> postRepository, IMongoRepository<Author> authorRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<Result<PageResult<Response.PostListResponse>>> Handle(Contract.Services.V1.Posts.Query.GetListPostsQuery request, CancellationToken cancellationToken)
    {

        SortOrder sort = SortOrderExtension.ConvertStringToSortOrder(request.SortOrder);
        IDictionary<string, SortOrder> sortColumnAndOrderDict = SortOrderExtension.ConvertStringToSortColumnOrder<Post>(request.SortColumnAndOrder);

        var queryable = _postRepository
            .AsQueryable(null);

        // Search
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            queryable = queryable.Where(x => x.Title.Contains(request.SearchTerm)
                                  || x.Content.Contains(request.SearchTerm));
        }

        // Sort
        queryable = sortColumnAndOrderDict.Any()
            ? SortOrderExtension.ApplyMultiColumnSorting(queryable, sortColumnAndOrderDict)
            : sort == SortOrder.Ascending ? queryable.OrderBy(GetSortColumnProterty(request)) :
                queryable.OrderByDescending(GetSortColumnProterty(request));

        // Paging
        var queryResult = await MongoPagingExtensions.ToPageResultAsync(
            queryable,
            request.PageIndex,
            request.PageSize
        );

        var final = queryResult.Map(p => new Response.PostListResponse(
                Id: p.DocumentId,
                Title: p.Title,
                Slug: p.Slug,
                Content: p.Content,
                PostAuthor: new PostAuthorViewModel
                {
                    //id = p.Author.Id,
                    Name = p.Author.Name,
                    UserName = p.Author.UserName,
                    Email = p.Author.Email,
                    AvatarUrl = p.Author.AvatarUrl
                }
                //PostTags: p.Tags.Select(t => new PostTagViewModel
                //{
                //    //Id = t.Id,
                //    Name = t.Name,
                //    Slug = t.Slug,
                //    Color = t.Color
                //}).ToList()
            ));

        return Result.Success(final);
    }

    public static Expression<Func<Post, object>> GetSortColumnProterty(Contract.Services.V1.Posts.Query.GetListPostsQuery request)
    {
        return request.SortColumn?.ToLower() switch
        {
            "title" => p => p.Title,
            "content" => p => p.Content,
            _ => p => p.Id
            // REAL: product => product.CreatedDate by default
        };
    }
}
