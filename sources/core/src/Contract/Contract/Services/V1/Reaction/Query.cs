using Contract.Abstractions.Message;

namespace Contract.Services.V1.Reaction;
public static class Query
{
    public record GetReactionsQuery() : IQuery<List<Response.Reaction>>;

}
