using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Domain.Exceptions;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetProductByIdQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetPostByIdQuery, Response.PostResponse>
{
    private readonly IMongoRepository<PostProjection> _postRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IMongoRepository<PostProjection> postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.PostResponse>> Handle(Contract.Services.V1.Posts.Query.GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.FindOneAsync(p => p.DocumentId == request.PostId)
            ?? throw new PostException.ProductNotFoundException(request.PostId);

        var result = new Response.PostResponse(post.DocumentId, post.Title, post.Content);

        return Result.Success(result);
    }
}
