using Contract.Abstractions.Message;
using Contract.Abstractions.Shared;
using Contract.Enumerations;

namespace Contract.Services.V1.Posts;
public static class Query
{
    public record GetAllPostsQuery() : IQuery<List<Response.PostResponse>>;
    public record GetListPostsQuery(string? SearchTerm, string? SortColumn, string? SortOrder, string? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PageResult<Response.PostResponse>>;
    public record GetPostByIdQuery(Guid PostId) : IQuery<Response.PostResponse>;
}

