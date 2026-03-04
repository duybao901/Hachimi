namespace Contract.Services.V1.Authors;

public static class Response
{
    public record AuthorStatsResponse(int TotalReactions, int TotalComments, int TotalViews, int TotalPosts, int TotalDrafts);
}
