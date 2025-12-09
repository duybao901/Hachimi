using Asp.Versioning;
using AuthorizationAPI.Abstractions;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommandV1 = Contract.Services.V1.Identity.Command;

namespace AuthorizationAPI.Controllers.V1;

[ApiVersion(1)]
public class AuthenController : ApiController
{

    public AuthenController(ISender sender) : base(sender)
    {
    }

    [HttpPost("login", Name = "Login")]
    [AllowAnonymous]
    public async Task<IResult> Login([FromBody] Query.Login login)
    {
        Result result = await Sender.Send(login);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpPost("register", Name = "register")]
    [AllowAnonymous]
    public async Task<IResult> Register([FromBody] CommandV1.RegisterUserCommand request)
    {
        Result result = await Sender.Send(request);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpPost("refresh-token", Name = "Refresh Token")]
    [Authorize]
    public async Task<IResult> RefreshToken([FromBody] Query.Token token)
    {
        var AccessToken = await HttpContext.GetTokenAsync("access_token");

        var result = await Sender.Send(new Query.Token(AccessToken, token.RefreshToken));

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpPost("revoke-token", Name = "Revoke Token")]
    [Authorize]
    public async Task<IResult> RevokeToken()
    {
        var AccessToken = await HttpContext.GetTokenAsync("access_token");

        var result = await Sender.Send(new CommandV1.RevokeToken(AccessToken));

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Results.Ok(result);
    }

    [HttpGet(Name = "Test authenticated")]
    [Authorize]
    public IResult TestAuthen()
    {
        return Results.Ok("You are authenticated");
    }
}
