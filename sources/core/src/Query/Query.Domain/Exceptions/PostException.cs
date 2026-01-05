namespace Query.Domain.Exceptions;
public class PostException
{
    public class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(Guid id)
            : base($"The post with the id {id} was not found")
        {
        }
    }

    public class PostNameWrongFormat : BadRequestException
    {
        public PostNameWrongFormat(string name)
            : base($"The post name '{name}' must contant 'product' in name.")
        {
        }
    }
}
