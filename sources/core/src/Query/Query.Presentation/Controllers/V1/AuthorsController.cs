using Contract.Abstractions.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Query.Presentation.Abstractions;
using Contract.Services.V1.Authors;

namespace Query.Presentation.Controllers.V1;

public class AuthorsController : ApiController
{
    public AuthorsController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{authorId}/stats")]
    [ProducesResponseType(typeof(Result<Response.AuthorStatsResponse>), StatusCodes.Status200OK)]
    public async Task<IResult> GetAuthorStats(Guid authorId)
    {
        var query = new Contract.Services.V1.Authors.Query.GetAuthorStatsQuery(authorId);
        var result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
