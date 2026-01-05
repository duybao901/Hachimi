using Contract.Abstractions.Message;

namespace Contract.Services.V1.Tags;
public class Query
{
    public record GetAllTags() : IQuery<List<Response.TagResponse>>;
    public record GetTagById(Guid Id) : IQuery<Response.TagResponse>;
}
