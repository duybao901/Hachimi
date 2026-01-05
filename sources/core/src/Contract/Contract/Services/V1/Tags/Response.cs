namespace Contract.Services.V1.Tags;
public static class Response
{
    public record TagResponse(Guid Id, string Name, string Description, string Color);
}
