using Contract.Abstractions.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Query.Presentation.Abstractions;

// custom ver (api/testversion{myVersion:apiVersion}/[controller])=> http://localhost:5051/api/testversion1/Product

[ApiController]
[Route("api/v{myVersion:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender) => Sender = sender;

    protected static IResult HandlerFailure(Result result) =>
       result switch
       {
           { IsSuccess: true } => throw new InvalidOperationException(),
           IValidationResult validationResult =>
               Results.BadRequest(
                   CreateProblemDetails(
                       "Validation Error",
                       StatusCodes.Status400BadRequest,
                       result.Error,
                       validationResult.Errors)),
           _ => Results.BadRequest(
                   CreateProblemDetails(
                       "Bad Request",
                       StatusCodes.Status400BadRequest,
                       result.Error))
       };

    private static ProblemDetails CreateProblemDetails(string title, int status, Error error, Error[]? errors = null)
    => new()
    {
        Title = title,
        Type = error.Code,
        Detail = error.Message,
        Status = status,
        Extensions = { { nameof(errors), errors } }
    };
}
