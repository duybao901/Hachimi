using AutoMapper;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Exceptions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;

namespace Command.Application.UserCases.V1.Queries.Post;
public class GetPostByIdQueryHandler : IQueryHandler<Query.GetPostByIdQuery, Response.PostResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Posts, Guid> _postRepositoryBase;
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(IRepositoryBase<Domain.Entities.Posts, Guid> postRepositoryBase, IMapper mapper)
    {
        _postRepositoryBase = postRepositoryBase;
        _mapper = mapper;
    }

    public async Task<Result<Response.PostResponse>> Handle(Query.GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepositoryBase.FindByIdAsync(request.PostId, cancellationToken) 
            ?? throw new PostException.PostNotFoundException(request.PostId);

        var result = _mapper.Map<Response.PostResponse>(post);

        return Result.Success(result);
    }
}
