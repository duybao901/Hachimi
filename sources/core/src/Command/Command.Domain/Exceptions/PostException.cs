namespace Command.Domain.Exceptions;
public class PostException
{
    public class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(Guid id)
            : base($"The post with the id {id} was not found")
        {
        }
    }
}
