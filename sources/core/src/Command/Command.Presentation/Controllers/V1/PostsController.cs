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
        var result = await Sender.Send(request);

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

    [HttpPost("save-draft")]
    public async Task<IActionResult> SaveDraftPost([FromBody] Contract.Services.V1.Posts.Command.SaveDraftPostCommand request)
    {   
        var result = await Sender.Send(request);

        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }
}
