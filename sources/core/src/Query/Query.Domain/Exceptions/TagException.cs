namespace Query.Domain.Exceptions;
public static class TagException
{
    public class TagNotFoundException : NotFoundException
    {
        public TagNotFoundException(Guid id)
            : base($"The post with the id {id} was not found")
        {
        }
    }
}
