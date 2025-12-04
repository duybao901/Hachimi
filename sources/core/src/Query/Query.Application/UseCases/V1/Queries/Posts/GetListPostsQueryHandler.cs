using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Enumerations;
using Contract.Extensions;
using Contract.Services.V1.Posts;
using MongoDB.Driver.Linq;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using System.Linq.Expressions;

namespace Query.Application.UseCases.V1.Queries.Posts;
public class GetListPostsQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetListPostsQuery, PageResult<Response.PostResponse>>
{
    private readonly IMongoRepository<PostProjection> _postRepository;
    private readonly IMapper _mapper;

    public GetListPostsQueryHandler(IMongoRepository<PostProjection> postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<PageResult<Response.PostResponse>>> Handle(Contract.Services.V1.Posts.Query.GetListPostsQuery request, CancellationToken cancellationToken)
    {

        SortOrder sort = SortOrderExtension.ConvertStringToSortOrder(request.SortOrder);
        IDictionary<string, SortOrder> sortColumnAndOrderDict = SortOrderExtension.ConvertStringToSortColumnOrder<PostProjection>(request.SortColumnAndOrder);

        var queryable = _postRepository
            .AsQueryable(null);

        // Search
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            queryable = queryable.Where(x => x.Title.Contains(request.SearchTerm)
                                  || x.Content.Contains(request.SearchTerm));
        }

        // Sort
        queryable = request.SortColumnAndOrder.Any()
            ? SortOrderExtension.ApplyMultiColumnSorting(queryable, sortColumnAndOrderDict)
            : request.SortOrder == SortOrder.Ascending ? queryable.OrderBy(GetSortColumnProterty(request)) :
                queryable.OrderByDescending(GetSortColumnProterty(request));

        // Paging
        var queryResult = await MongoPagingExtensions.ToPageResultAsync(
            queryable,
            request.PageIndex,
            request.PageSize
        );

        var final = queryResult.Cast(p => new Response.PostResponse(
                p.DocumentId,
                p.Title,
                p.Content
            ));

        return Result.Success(final);
    }

    public static Expression<Func<PostProjection, object>> GetSortColumnProterty(Contract.Services.V1.Posts.Query.GetListPostsQuery request)
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
