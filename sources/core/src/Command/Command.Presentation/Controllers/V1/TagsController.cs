using Command.Presentation.Abstractions;
using MediatR;
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
    public async Task<IActionResult> CreateTags([FromBody] CommandV1.CreateTagCommand request)
    {
        var result = await Sender.Send(request);
        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Update a tag
    /// </summary>
    [HttpPut("{tagId}")]
    public async Task<IActionResult> UpdateTags(Guid tagId, [FromBody] CommandV1.UpdateTagCommand request)
    {
        var result = await Sender.Send(new Contract.Services.V1.Tags.Command.UpdateTagCommand(tagId, request.Name, request.Color));
        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Delete a tag
    /// </summary>
    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTags(Guid tagId)
    {
        var result = await Sender.Send(new Contract.Services.V1.Tags.Command.DeleteTagCommand(tagId));
        if (result.IsFailure)
        {
            HandlerFailure(result);
        }

        return Ok(result);
    }
}
