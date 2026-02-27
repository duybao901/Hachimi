using AutoMapper;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;
using Query.Domain.Exceptions;
using Query.Application.Abstraction;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetProductByIdQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetPostByIdQuery, Response.PostResponse>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetProductByIdQueryHandler(IMongoRepository<Post> postRepository, IMapper mapper, ICurrentUser currentUser)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Result<Response.PostResponse>> Handle(Contract.Services.V1.Posts.Query.GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.FindOneAsync(p => p.DocumentId == request.PostId)
            ?? throw new PostException.PostNotFoundException(request.PostId);

        var result = new Response.PostResponse(
            post.DocumentId,
            post.Title,
            post.Slug,
            post.Content.ToString(),
            new PostAuthorViewModel()
            {
                Id = post.Author.DocumentId,
                Name = post.Author.Name,
                UserName = post.Author.UserName,
                Email = post.Author.Email,
                AvatarUrl = post.Author.AvatarUrl
            },
            PostTags: post.Tags
                    .Select(t => new PostTagViewModel
                    {
                        Id = t.DocumentId,
                        Name = t.Name,
                        Description = t.Description,
                        Color = t.Color
                    })
                    .ToList(),
            Reactions: post.Reactions.Select(r => new Contract.Services.V1.Reaction.ViewModels.ReactionViewModel
                    {
                        Id = r.DocumentId,
                        Name = r.Name,
                        Icon = r.Icon,
                        Url = r.Url,
                        Count = r.Count,
                        IsReactionByCurrentUser = !string.IsNullOrEmpty(_currentUser?.UserId) && r.UserIds != null && r.UserIds.Contains(_currentUser.UserId)
                    }).ToList()
            );

        return Result.Success(result);
    }
}
