using Asp.Versioning;
using Command.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CommandV1 = Contract.Services.V1.Posts.Command;

namespace Command.Presentation.Controllers.V1;

[ApiVersion(1)]
public class PostsController : ApiController
{
    public PostsController(ISender sender) : base(sender)
    {
    }

    ///// <summary>
    ///// Get All Posts
    ///// </summary>
    ///// <remarks>
    /////     Sample request:
    /////     GET /api/v1/Post
    ///// </remarks>
    //[HttpGet]
    //[ProducesResponseType(typeof(Result<IEnumerable<Response.PostResponse>>), StatusCodes.Status200OK)]

    //public async Task<IActionResult> GetAllPosts()
    //{
    //    var query = new Query.GetAllPostsQuery();
    //    var result = await Sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        HandlerFailure(result);
    //    }

    //    return Ok(result);
    //}

    ///// <summary>
    ///// Get Post by Id
    ///// </summary>
    ///// <remarks>
    /////     Sample request:
    /////     GET /api/v1/Post/{postId}
    ///// </remarks>
    //[HttpGet("{postId}")]
    //[ProducesResponseType(typeof(Result<IEnumerable<Response.PostResponse>>), StatusCodes.Status200OK)]

    //public async Task<IActionResult> GetPostById(Guid postId)
    //{
    //    var query = new Query.GetPostByIdQuery(postId);
    //    var result = await Sender.Send(query);

    //    if (result.IsFailure)
    //    {
    //        HandlerFailure(result);
    //    }

    //    return Ok(result);
    //}

    /// <summary>
    /// Create a post
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CommandV1.CreatePostCommand request)
    {
        var result = await Sender.Send(request);
        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Update a post
    /// </summary>
    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid postId, [FromBody] CommandV1.UpdatePostCommand request)
    {
        var command = new CommandV1.UpdatePostCommand(postId, request.Title, request.Content, request.TagIds, request.CoverImageUrl);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Delete a post
    /// </summary>
    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        var command = new CommandV1.DeletePostCommand(postId);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Publish a post
    /// </summary>
    [HttpPut("{postId}/publish")]
    public async Task<IActionResult> PublishPost(Guid postId)
    {
        var command = new CommandV1.PublishPostCommand(postId);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    [HttpGet("draft")]
    public async Task<IActionResult> GetOrCreateDraft()
    {

        var result = await Sender.Send(new CommandV1.GetOrCreateDraftCommand());

        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    [HttpGet("save-draft")]
    public async Task<IActionResult> SaveDraftPost([FromBody] CommandV1.SaveDraftPost request)
    {
        var result = await Sender.Send(request);

        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }
}
