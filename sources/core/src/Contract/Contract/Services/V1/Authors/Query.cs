using Contract.Abstractions.Message;

namespace Contract.Services.V1.Authors;

public static class Query
{
    public record GetAuthorStatsQuery(Guid AuthorId) : IQuery<Response.AuthorStatsResponse>;
}
