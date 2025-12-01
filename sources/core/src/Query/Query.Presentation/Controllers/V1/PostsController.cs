using Asp.Versioning;
using Contract.Abstractions.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Query.Presentation.Abstractions;
using QueryV1 = Contract.Services.V1.Posts.Query;

namespace Query.Presentation.Controllers.V1;

[ApiVersion("1.0")]
public class PostsController : ApiController
{
    public PostsController(ISender sender) : base(sender)
    {
    }


    [HttpGet(Name = "GetPosts")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> GetAllPosts()
    {
        var query = new QueryV1.GetAllPostsQuery();
        Result result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpGet("{postId}")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> GetPostById(Guid postId)
    {
        var query = new QueryV1.GetPostByIdQuery(postId);
        Result result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
