using Contract.Abstractions.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.V1;
public class AuthorsController : ApiController

{
    public AuthorsController(ISender sender) : base(sender)
    {
    }

    //[HttpPost('me')]
    //[ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    //public async Task<IResult> GetCurrentUser()
    //{
    //    var query = new QueryV1.GetAllPostsQuery();
    //    Result result = await Sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        return HandlerFailure(result);
    //    }

    //    return Results.Ok(result);
    //}

    //[HttpPost('info')]
    //[ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    //public async Task<IResult> GetCurrentUser()
    //{
    //    var query = new QueryV1.GetAllPostsQuery();
    //    Result result = await Sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        return HandlerFailure(result);
    //    }

    //    return Results.Ok(result);
    //}
}
