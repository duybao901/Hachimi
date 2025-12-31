namespace Command.Domain.Exceptions;
public class TagException
{
    public class TagNotFoundException : NotFoundException
    {
        public TagNotFoundException(Guid id)
            : base($"The tag with the id {id} was not found")
        {
        }
    }

    public class TagAlreadyExitsInAnotherPost : BadRequestException
    {
        public TagAlreadyExitsInAnotherPost()
            : base($"Can't delete the tag because already exits in another post")
        {
        }
    }
}
