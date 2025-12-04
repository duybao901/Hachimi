namespace Contract.Services.V1.Posts;
public static class Response
{
    public record PostResponse(Guid Id, string Title, string Content);
}
