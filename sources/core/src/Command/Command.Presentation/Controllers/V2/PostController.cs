using Asp.Versioning;
using Command.Presentation.Abstractions;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.V2;

[ApiVersion(2)]
public class PostController : ApiController
{
    public PostController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.PostResponse>>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAllPostsV2(CancellationToken cancellationToken)
    {
        //var query = new Application.Queries.Post.GetAllPostsQuery();
        //var result = await Sender.Send(query, cancellationToken);
        return Ok("Posts...");
    }
}
