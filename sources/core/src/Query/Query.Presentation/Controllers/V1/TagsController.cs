using Contract.Abstractions.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Query.Presentation.Abstractions;
using QueryV1 = Contract.Services.V1.Tags.Query;

namespace Query.Presentation.Controllers.V1;
public class TagsController : ApiController
{
    public TagsController(ISender sender) : base(sender)
    {
    }


    [HttpGet]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> SearchTags([FromQuery] string? searchTerm = null)
    {
        var query = new QueryV1.SearchTags(searchTerm);
        Result result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpGet("{tagId}")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> GetTagById(Guid tagId)
    {
        var query = new QueryV1.GetTagById(tagId);
        Result result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
