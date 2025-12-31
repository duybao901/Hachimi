using Command.Presentation.Abstractions;
using MediatR;

namespace Command.Presentation.Controllers.V1;
public class TagsController : ApiController
{
    public TagsController(ISender sender) : base(sender)
    {
    }
}
