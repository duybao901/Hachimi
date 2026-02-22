namespace Command.Domain.Exceptions;
public static class ReactionException
{
    public class ReactionNotFoundException : NotFoundException
    {
        public ReactionNotFoundException(Guid id)
            : base($"The react with the id {id} was not found")
        {
        }
    }
}
