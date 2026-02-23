namespace Contract.Services.V1.Reaction;
public static class Response
{
    public record Reaction(Guid Id, string Name, string Icon);
}
