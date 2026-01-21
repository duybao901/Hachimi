using Command.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommandV1 = Contract.Services.V1.Tags.Command;

namespace Command.Presentation.Controllers.V1;
public class TagsController : ApiController
{
    public TagsController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    /// Create a tag
    /// </summary>
    [HttpPost]
    public async Task<IResult> CreateTags([FromBody] CommandV1.CreateTagCommand request)
    {
        var result = await Sender.Send(request);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Update a tag
    /// </summary>
    [HttpPut("{tagId}")]
    public async Task<IResult> UpdateTags(Guid tagId, [FromBody] CommandV1.UpdateTagCommand request)
    {
        var result = await Sender.Send(new CommandV1.UpdateTagCommand(tagId, request.Name, request.Description, request.Color));
        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    /// <summary>
    /// Delete a tag
    /// </summary>
    [HttpDelete("{tagId}")]
    public async Task<IResult> DeleteTags(Guid tagId)
    {
        var result = await Sender.Send(new CommandV1.DeleteTagCommand(tagId));
        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Results.Ok(result);
    }
}
