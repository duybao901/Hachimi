using MediatR;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.V1;
public class TagsController : ApiController
{
    public TagsController(ISender sender) : base(sender)
    {
    }
}
