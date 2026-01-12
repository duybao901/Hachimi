using Contract.Abstractions;
using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetCurrentEditPostQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetCurrentEditPost, Response.PostCurrentEditReponse>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly ICurrentUser _currentUser;

    public GetCurrentEditPostQueryHandler(IMongoRepository<Post> postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository;
        _currentUser = currentUser;
    }
    public async Task<Result<Response.PostCurrentEditReponse>> Handle(Contract.Services.V1.Posts.Query.GetCurrentEditPost request, CancellationToken cancellationToken)
    {
        var postCurrentEdit = await _postRepository.FindOneAsync(p => p.IsPublished);
        var userId = _currentUser.UserId;

        if (postCurrentEdit == null)
        {
            var initialCurrentEditPost = new Response.PostCurrentEditReponse(Guid.NewGuid(), "", "", userId, []);

            return Result.Success(initialCurrentEditPost);
        }

        var currentEditPost = new Response.PostCurrentEditReponse(Guid.NewGuid(), postCurrentEdit.Title, postCurrentEdit.Content, userId, []);

        return Result.Success(currentEditPost);
    }
}
