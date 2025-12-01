namespace Query.Domain.Exceptions;
public class PostException
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid id)
            : base($"The post with the id {id} was not found")
        {
        }
    }

    public class ProductNameWrongFormat : BadRequestException
    {
        public ProductNameWrongFormat(string name)
            : base($"The post name '{name}' must contant 'product' in name.")
        {
        }
    }
}
