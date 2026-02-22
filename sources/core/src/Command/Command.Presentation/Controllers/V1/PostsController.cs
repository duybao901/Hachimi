using Asp.Versioning;
using Command.Presentation.Abstractions;
using Contract.Abstractions.Shared;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Contract.Services.V1.Posts.Command;
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
    public async Task<IResult> CreatePost([FromBody] CommandV1.CreatePostCommand request)
    {
        var result = await Sender.Send(request);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Update a post
    /// </summary>
    [HttpPut("{postId}")]
    public async Task<IResult> UpdatePost(Guid postId, [FromBody] CommandV1.UpdatePostCommand request)
    {
        var result = await Sender.Send(request);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Delete a post
    /// </summary>
    [HttpDelete("{postId}")]
    public async Task<IResult> DeletePost(Guid postId)
    {
        var command = new CommandV1.DeletePostCommand(postId);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Publish draft post
    /// </summary>
    [HttpPut("{postId}/publish")]
    public async Task<IResult> PublishDraftPost(Guid postId)
    {
        var command = new CommandV1.PublishDraftPostCommand(postId);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpPost("save-draft")]
    public async Task<IResult> SaveDraftPost([FromBody] Contract.Services.V1.Posts.Command.SaveDraftPostCommand request)
    {   
        var result = await Sender.Send(request);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpPost("publish")]
    public async Task<IResult> PublishPost([FromBody] Contract.Services.V1.Posts.Command.PublishPostCommand request)
    {
        var result = await Sender.Send(request);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    //[HttpPost("{postId}/like")]
    //public async Task<IActionResult> LikePost(Guid postId, [FromBody] Contract.Services.V1.Posts.Command.AddCommentCommand request)
    //{
    //    var result = await Sender.Send(request);

    //    if (result.IsFailure)
    //    {
    //        return HandlerFailure(result);
    //    }

    //    return Results.Ok(result);
    //}

    [HttpPost("{postId}/like")]
    public async Task<IResult> LikePost(Guid postId, [FromBody] Contract.Services.V1.Posts.Command.AddReactionCommand request)
    {
        var result = await Sender.Send(new AddReactionCommand(postId, request.UserId, request.ReactionTypeId));

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
