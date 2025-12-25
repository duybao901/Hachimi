using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetAllPostsQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetAllPostsQuery, List<Response.PostResponse>>
{
    private readonly IMongoRepository<PostProjection> _postRepository;
    private readonly IMapper _mapper;

    public GetAllPostsQueryHandler(IMongoRepository<PostProjection> postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<Response.PostResponse>>> Handle(Contract.Services.V1.Posts.Query.GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.FindAll();

        var result = new List<Response.PostResponse>();

        foreach (var post in posts)
        {
            result.Add(new Response.PostResponse(post.DocumentId,
                post.Title, 
                post.Slug, 
                post.Content, 
                new PostAuthorViewModel()
                {
                    Name = post.Author.Name,
                    Email = post.Author.Email,
                })
            );
        }

        return Result.Success(result);
    }
}
