using ApiGateway.Abstractions;
using Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace ApiGateway.Attributes;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CustomJwtBearerEvents> _logger;

    public CustomJwtBearerEvents(ICacheService cacheService, ILogger<CustomJwtBearerEvents> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    //public override async Task TokenValidated(TokenValidatedContext context)
    //{
    //    // JsonWebToken accessToken = context.SecurityToken as JsonWebToken;
    //    // ✅ Kiểm tra và ép kiểu trong 1 bước: Down-cast the property to JsonWebToken
    //    if (context.SecurityToken is JsonWebToken accessToken)
    //    {
    //        var requestToken = accessToken.EncodedToken;

    //        var emailKey = accessToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
    //        var authenticated = await _cacheService.GetAsync<Response.Authenticated>(emailKey);

    //        if (authenticated is null || authenticated.AccessToken != requestToken)
    //        {
    //            context.Response.Headers.Add("IS-TOKEN-REVOKED", "true");
    //            context.Fail("Authentication fail. Token has been revoked!");
    //        }
    //    }
    //    else
    //    {
    //        context.Fail("Authentication fail.");
    //    }
    //}

    public override Task Challenge(JwtBearerChallengeContext context)
    {
        if (context.Error == "invalid_token" &&
            context.ErrorDescription?.Contains("expired", StringComparison.OrdinalIgnoreCase) == true)
        {
            _logger.LogInformation("Token expired.");
            context.Response.Headers["IS-TOKEN-EXPIRED"] = "true";
        }

        // Importance: Overide JwtBearer default response
        context.HandleResponse();

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }
}
