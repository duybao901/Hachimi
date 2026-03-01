using Asp.Versioning;
using Command.Application.Abstractions;
using Command.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.V1;

[ApiVersion(1)]
public class FilesController : ApiController
{
    private readonly IImageService _imageService;

    public FilesController(ISender sender, IImageService imageService) : base(sender)
    {
        _imageService = imageService;
    }

    /// <summary>
    /// Upload an image to Cloudinary
    /// </summary>
    [HttpPost("image")]
    public async Task<IResult> UploadImage(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return Results.BadRequest("No file uploaded.");
        }

        try
        {
            var url = await _imageService.UploadImageAsync(file, cancellationToken);
            return Results.Ok(new { secure_url = url });
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
