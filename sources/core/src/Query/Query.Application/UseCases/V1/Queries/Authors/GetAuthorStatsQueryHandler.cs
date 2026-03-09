using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Authors;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using MongoDB.Driver.Linq;

namespace Query.Application.UseCases.V1.Queries.Authors;

public sealed class GetAuthorStatsQueryHandler : IQueryHandler<Contract.Services.V1.Authors.Query.GetAuthorStatsQuery, Response.AuthorStatsResponse>
{
    private readonly IMongoRepository<Post> _postRepository;

    public GetAuthorStatsQueryHandler(IMongoRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Result<Response.AuthorStatsResponse>> Handle(Contract.Services.V1.Authors.Query.GetAuthorStatsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _postRepository.AsQueryable(p => p.Author.DocumentId == request.AuthorId);
        
        var totalReactions = await queryable
            .SelectMany(p => p.Reactions)
            .SumAsync(r => r.Count, cancellationToken);
            
        var totalComments = await queryable.SumAsync(p => p.CommentCount, cancellationToken);
        var totalViews = await queryable.SumAsync(p => p.ViewCount, cancellationToken);
        var totalPosts = await queryable.CountAsync(cancellationToken);
        var totalDrafts = await queryable.CountAsync(p => p.PostStatus == Contract.Enumerations.PostStatus.Draft, cancellationToken);

        return Result.Success(new Response.AuthorStatsResponse(totalReactions, totalComments, totalViews, totalPosts, totalDrafts));
    }
}
