using Asp.Versioning;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
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


    [HttpGet("public")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> GetAllPosts(
        int pageIndex = 1,
        int pageSize = 10,
        string? feed = "")
    {
        var query = new QueryV1.GetPostsQuery(pageIndex, pageSize, feed);
        var result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> GetListPosts(
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        string? sortColumnAndOrder = null,
        int pageIndex = 1,
        int pageSize = 5
    )
    {
        Result<PageResult<Response.PostListResponse>> result = await Sender.Send(new QueryV1.GetListPostsQuery(
            searchTerm, sortColumn, sortOrder, sortColumnAndOrder, pageIndex, pageSize));

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

    [HttpGet("current-edit")]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    public async Task<IResult> GetCurrentEditPost()
    {
        var query = new QueryV1.GetCurrentEditPost();
        Result result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
