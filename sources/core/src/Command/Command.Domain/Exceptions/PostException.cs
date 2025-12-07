namespace Command.Domain.Exceptions;
public static class PostException
{
    public class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(Guid id)
            : base($"The post with the id {id} was not found")
        {
        }      
    }

    public class MinimumTagsRequiredException : DomainException
    {
        public MinimumTagsRequiredException(int minimumTagsRequied)
            : base("Tag requirement", $"A post must contain at least {minimumTagsRequied} tags.")
        {
        }        
    }
}
