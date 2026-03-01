using Microsoft.AspNetCore.Http;

namespace Command.Application.Abstractions;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);
}
