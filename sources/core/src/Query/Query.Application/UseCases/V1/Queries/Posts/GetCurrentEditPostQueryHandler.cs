using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Application.Abstraction;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Collections;

namespace Query.Application.UseCases.V1.Queries.Posts;
public sealed class GetCurrentEditPostQueryHandler : IQueryHandler<Contract.Services.V1.Posts.Query.GetCurrentEditPost, Response.PostCurrentEditReponse>
{
    private readonly IMongoRepository<Post> _postRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMongoRepository<Author> _authorRepository;

    public GetCurrentEditPostQueryHandler(ICurrentUser currentUser, IMongoRepository<Post> postRepository, IMongoRepository<Author> authorRepository)
    {
        _postRepository = postRepository;
        _currentUser = currentUser;
        _authorRepository = authorRepository;
    }
    public async Task<Result<Response.PostCurrentEditReponse>> Handle(Contract.Services.V1.Posts.Query.GetCurrentEditPost request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        var author = await _authorRepository.FindOneAsync(author => author.UserId == userId);

        var postCurrentEdit = await _postRepository.FindOneAsync(p => p.Author.DocumentId == author.DocumentId & p.IsPostEditing);
       
        if (postCurrentEdit == null)
        {
            var initialCurrentEditPost = new Response.PostCurrentEditReponse(Guid.NewGuid(), "", "", userId, []);

            return Result.Success(initialCurrentEditPost);
        }

        var currentEditPost = new Response.PostCurrentEditReponse(Guid.NewGuid(), postCurrentEdit.Title, postCurrentEdit.Content, userId, []);

        return Result.Success(currentEditPost);
    }
}
