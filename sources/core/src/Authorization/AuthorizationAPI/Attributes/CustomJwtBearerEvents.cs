using AuthorizationApi.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuthorizationApi.Attributes;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CustomJwtBearerEvents> _logger;

    public CustomJwtBearerEvents(ICacheService cacheService, ILogger<CustomJwtBearerEvents> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }


    // Check REVOKED-TOKEN -> move to AuthorizationAPI refresh token api
    //public override async Task TokenValidated(TokenValidatedContext context)
    //{
    //    if (context.SecurityToken is not JsonWebToken jwt)
    //    {
    //        context.Fail("Invalid token format");
    //        return;
    //    }

    //    var email = jwt.Claims
    //        .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    //    if (string.IsNullOrEmpty(email))
    //    {
    //        context.Fail("Missing email claim");
    //        return;
    //    }

    //    var session = await _cacheService
    //        .GetAsync<Response.Authenticated>(email);

    //    if (session is null || session.AccessToken != jwt.EncodedToken)
    //    {
    //        context.Response.Headers["IS-TOKEN-REVOKED"] = "true";
    //        context.Fail("Token revoked");
    //    }
    //}

    // Check TOKEN-EXPIRED
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
