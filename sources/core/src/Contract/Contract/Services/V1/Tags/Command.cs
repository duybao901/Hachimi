using Contract.Abstractions.Message;

namespace Contract.Services.V1.Tags;
public static class Command
{
    public record CreateTagCommand(Guid Id, string Name, string Color) : ICommand;
    public record UpdateTagCommand(Guid Id, string Name, string Color) : ICommand;
    public record DeleteTagCommand(Guid Id) : ICommand;
}
