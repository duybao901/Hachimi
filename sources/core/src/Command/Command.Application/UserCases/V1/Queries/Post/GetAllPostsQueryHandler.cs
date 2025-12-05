using AutoMapper;
using Command.Domain.Abstractions.Repositories;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Microsoft.EntityFrameworkCore;
using static Contract.Services.V1.Posts.Query;

namespace Command.Application.UserCases.V1.Queries.Post;
public sealed class GetAllPostsQueryHandler : IQueryHandler<Query.GetAllPostsQuery, List<Response.PostResponse>>
{
    private readonly IRepositoryBase<Command.Domain.Entities.Posts, Guid> _postRepository;
    private readonly IMapper _mapper;


    public GetAllPostsQueryHandler(IRepositoryBase<Command.Domain.Entities.Posts, Guid> postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<Response.PostResponse>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.FindAll().ToListAsync();

        var result = _mapper.Map<List<Response.PostResponse>>(posts);

        return Result.Success(result);
    }
}
