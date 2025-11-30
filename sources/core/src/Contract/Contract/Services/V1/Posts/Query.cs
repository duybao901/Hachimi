using Contract.Abstractions.Message;

namespace Contract.Services.V1.Posts;
public static class Query
{
    public record GetAllPostsQuery() : IQuery<List<Response.PostResponse>>;
    public record GetPostByIdQuery(Guid PostId) : IQuery<Response.PostResponse>;
}

