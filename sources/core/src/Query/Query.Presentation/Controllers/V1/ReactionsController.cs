using Contract.Abstractions.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Query.Presentation.Abstractions;
using QueryV1 = Contract.Services.V1.Reaction.Query;

namespace Query.Presentation.Controllers.V1;
public class ReactionsController : ApiController
{
    public ReactionsController(ISender sender) : base(sender)
    {
    }

    [HttpGet("public")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> SearchTags()
    {
        var query = new QueryV1.GetReactionsQuery();
        Result result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
